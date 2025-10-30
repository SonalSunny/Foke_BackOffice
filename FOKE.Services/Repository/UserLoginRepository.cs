using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.Options;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly FOKEDBContext _dbContext;
        private readonly ILogger<UserLoginRepository> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public UserLoginRepository(FOKEDBContext SkakDBContext, IMemoryCache cache, IHttpContextAccessor httpContextAccessor, ILogger<UserLoginRepository> logger)
        {
            _memoryCache = cache;
            _dbContext = SkakDBContext;
            _httpContextAccessor = httpContextAccessor;
            claimsPrincipal = _httpContextAccessor?.HttpContext?.User as ClaimsPrincipal;
            _logger = logger;
        }

        public async Task<ResponseEntity<UserViewModel>> Login(LoginViewModel model)
        {
            var retModel = new ResponseEntity<UserViewModel>();
            try
            {
                var objModel = new UserViewModel();
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.UserName == model.Username && x.Password == model.Password && x.Active == true);

                if (user == null)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Login failed.The email or password you entered is incorrect.";
                }

                else if (user != null)
                {
                    _memoryCache.TryGetValue($"ActiveUserSession_{user.UserId.ToString()}", out ActiveSessionOptions memoryData);
                    if (memoryData?.UserId != null && memoryData.LastActiveDateTime > DateTime.UtcNow.AddMinutes(-30) && (model.ForceLogin ?? false) == false)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.Conflict;
                        retModel.returnMessage = "User Session already exists";
                        //memoryData = memoryData == null ? new ActiveSessionOptions() : memoryData;
                    }
                    else
                    {
                        using var transaction = _dbContext.Database.BeginTransaction();
                        Guid obj = Guid.NewGuid();
                        var sessionId = obj.ToString();

                        user.ActiveSessionId = sessionId;
                        user.LastLoggedInDatetime = DateTime.UtcNow;
                        _dbContext.Entry(user).State = EntityState.Modified;

                        var userSessionData = new UserSession();
                        userSessionData.LoggedInUserId = user.UserId;
                        userSessionData.LoginDateTime = DateTime.UtcNow;
                        userSessionData.SessionId = sessionId;
                        userSessionData.LogoutDateTime = null;
                        await _dbContext.UserSessions.AddAsync(userSessionData);
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();

                        AddUpdateUserMemoryCache(sessionId, user.UserId);

                        var objUser = GetLoggedInUser(user.UserId);
                        retModel.returnData = objUser.returnData;
                        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                        retModel.returnMessage = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UserLoginRepository: {ex.Message}");
                throw new Exception("Error occurred while logging in!");
            }

            return retModel;
        }

        public async Task<bool> Logout()
        {
            try
            {
                var sessionId = claimsPrincipal?.FindFirst("LoginSessionId")?.Value;

                claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
                var isAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false;
                if (isAuthenticated)
                {
                    var userIdentity = claimsPrincipal?.Identity?.Name;
                    if (userIdentity != null)
                    {
                        long userid = 0;
                        Int64.TryParse(userIdentity, out userid);
                        if (userid > 0)
                        {
                            loggedInUser = userid;
                        }
                    }
                }

                if (loggedInUser > 0)
                {
                    var objUser = await _dbContext.Users.FirstOrDefaultAsync(c => c.UserId == loggedInUser && c.Active == true);
                    if (objUser != null)
                    {
                        objUser.ActiveSessionId = null;
                        if (!string.IsNullOrEmpty(sessionId))
                        {
                            var objUserLogin = await _dbContext.UserSessions.FirstOrDefaultAsync(c => c.LoggedInUserId == loggedInUser && c.SessionId == sessionId);
                            if (objUserLogin != null)
                            {
                                objUserLogin.LogoutDateTime = DateTime.UtcNow;
                            }
                            _dbContext.UserSessions.Update(objUserLogin);
                        }
                        await _dbContext.SaveChangesAsync();
                        _memoryCache.Remove($"ActiveUserSession_{loggedInUser.ToString()}");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        public bool AddUpdateUserMemoryCache(string activeSessionId, long userId)
        {
            try
            {
                var memoryCache = new MemoryCache(new MemoryCacheOptions());
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddMinutes(30), // Cache entry will expire after 30 minutes
                    Priority = CacheItemPriority.High, // Priority of the cache entry
                    SlidingExpiration = TimeSpan.FromMinutes(10) // Sliding expiration time
                };

                var memoryData = new ActiveSessionOptions();
                memoryData.UserId = userId;
                memoryData.ActiveSessionId = activeSessionId;
                memoryData.LastActiveDateTime = DateTime.UtcNow;

                _memoryCache.Remove($"ActiveUserSession_{userId.ToString()}");
                _memoryCache.Set($"ActiveUserSession_{userId.ToString()}", memoryData, cacheEntryOptions);
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public ResponseEntity<UserViewModel> GetLoggedInUser(long userId)
        {
            var retModel = new ResponseEntity<UserViewModel>();
            try
            {
                var objUser = _dbContext.Users.Where(c => c.UserId == userId).FirstOrDefault();
                var objModel = new UserViewModel();
                objModel.UserId = objUser.UserId;
                objModel.Username = objUser.UserName;
                objModel.Culture = objUser.Culture;
                objModel.ActiveSessionId = objUser.ActiveSessionId;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
    }
}
