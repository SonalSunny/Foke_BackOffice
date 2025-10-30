using FOKE.Entity;
using FOKE.Localization;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FOKE.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeViewModel LoginInfo { get; set; } = new HomeViewModel();
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        //public DateTime? CurrentLoginTime { get; set; }
        //public DateTime? PreviousLoginTime { get; set; }
        //public DateTime? PasswordExpiryDate { get; set; }
        public string LocalizedWelcome { get; private set; }


        public HomeModel(ISharedLocalizer sharedLocalizer, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _sharedLocalizer = sharedLocalizer;

            _httpContextAccessor = httpContextAccessor;

            _userRepository = userRepository;


        }
        private long? GetLoggedInUserId()
        {
            var claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
            if (claimsPrincipal?.Identity?.IsAuthenticated ?? false)
            {


                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
                if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId) && userId > 0)
                {
                    return userId;
                }
            }
            return null;
        }

        public async Task OnGetAsync()
        {
            var userId = GetLoggedInUserId();
            if (userId == null) return;

            // Call your new repository method that returns all login info and password expiry
            var loginInfo = await _userRepository.GetUserLoginInfoAsync(userId.Value);

            if (loginInfo != null)
            {
                LoginInfo = loginInfo;  // Assign directly to your view model property
            }
        }

    }
}















