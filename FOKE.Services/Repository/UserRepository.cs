using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.ChangePassword.ViewModel;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public UserRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;

            try
            {
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
            }
            catch (Exception ex)
            {
            }
        }
        public async Task<ResponseEntity<UserViewModel>> RegisterUser(UserViewModel model)
        {
            var retModel = new ResponseEntity<UserViewModel>();
            try
            {

                var userExists = _dbContext.Users
                        .Any(u => u.UserName == model.Username);
                if (userExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "User Already Exist";
                }
                else
                {
                    var user = new Users
                    {
                        UserName = model.Username,
                        Password = model.Password,
                        PasswordExpiryDate = model.PasswordExpirydate,
                        Active = true,
                        RoleId = model.RoleId,
                        CreatedBy = model.loggedinUserId//loginned user employee is
                    };
                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Registered Successfully";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }
        public ResponseEntity<List<UserViewModel>> GetAllUsers(long? Status, long? Roleid, DateTime? PasswordExpirydateFrom, DateTime? PasswordExpirydateTo)
        {
            var retModel = new ResponseEntity<List<UserViewModel>>();
            try
            {
                var objModel = new List<UserViewModel>();

                var retData = _dbContext.Users.Where(c => c.Active == true);

                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.Users.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.Users.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.Users.Where(c => c.Active == false);
                    }
                }

                if (Roleid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.RoleId == Roleid);
                }

                if (PasswordExpirydateFrom.HasValue)
                {
                    retData = retData.Where(c => c.PasswordExpiryDate >= PasswordExpirydateFrom.Value);
                }

                if (PasswordExpirydateTo.HasValue)
                {
                    retData = retData.Where(c => c.PasswordExpiryDate <= PasswordExpirydateTo.Value);
                }


                objModel = retData.Select(c => new UserViewModel()
                {
                    UserId = c.UserId,
                    Username = c.UserName,
                    Active = c.Active,
                    RoleId = c.RoleId ?? 0, // Handle nullable RoleId
                    RoleName = _dbContext.Roles
                        .Where(r => r.RoleId == c.RoleId)
                        .Select(r => r.RoleName)
                        .FirstOrDefault(),
                    PasswordExpirydate = c.PasswordExpiryDate,
                    CreatedDate = c.CreatedDate,
                    CreatedBy = _dbContext.Users
                        .Where(u => u.UserId == c.CreatedBy)
                        .Select(u => u.UserId)
                        .FirstOrDefault(),
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }
        public ResponseEntity<bool> DeleteUser(UserViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var memberDetails = _dbContext.Users.Find(objModel.UserId);
                if (objModel.DiffId == 1)
                {
                    memberDetails.Active = false;
                    _dbContext.Entry(memberDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    memberDetails.Active = true;
                    _dbContext.Entry(memberDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Activated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public ResponseEntity<UserViewModel> GetUserbyId(long userId)
        {
            var retModel = new ResponseEntity<UserViewModel>();
            try
            {
                var objUser = _dbContext.Users
                     .SingleOrDefault(u => u.UserId == userId);
                var objModel = new UserViewModel();
                objModel.UserId = objUser.UserId;
                objModel.Username = objUser.UserName;
                objModel.Password = objUser.Password;
                objModel.PasswordExpirydate = objUser.PasswordExpiryDate;
                objModel.PasswordExpirydateFormat = objUser.PasswordExpiryDate?.ToString("d-MMM-yyyy");
                objModel.Active = objUser.Active;
                objModel.RoleId = (long)objUser.RoleId;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public async Task<ResponseEntity<UserViewModel>> UpdateUser(UserViewModel model)
        {
            var retModel = new ResponseEntity<UserViewModel>();
            try
            {
                var user = _dbContext.Users.Where(c => c.UserId == model.UserId && c.Active == true).FirstOrDefault();
                if (user != null)
                {
                    var userExists = _dbContext.Users
                         .Any(u => u.UserId != model.UserId && u.UserName == model.Username);
                    if (userExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "User Already Exist";
                    }
                    else
                    {
                        user.UserName = model.Username;
                        user.RoleId = model.RoleId;
                        user.PasswordExpiryDate = DateTime.ParseExact(model.PasswordExpirydateFormat, "d-MMM-yyyy", CultureInfo.InvariantCulture);
                        user.UpdatedBy = model.loggedinUserId;
                        await _dbContext.SaveChangesAsync();
                        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                        retModel.returnMessage = "Updated Successfully";
                    }
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Invalid User";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }
        public ResponseEntity<string> ExportUserDatatoExcel(string search, long? statusid, long? Roleid, DateTime? PasswordExpirydateFrom, DateTime? PasswordExpirydateTo)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllUsers(statusid, Roleid, PasswordExpirydateFrom, PasswordExpirydateTo);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        // Set header row
                        var worksheet = workbook.Worksheets.Add("User Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Employee Name";
                        worksheet.Cell(1, 3).Value = "Employee Number";
                        worksheet.Cell(1, 4).Value = "User Name";
                        worksheet.Cell(1, 5).Value = "Role";
                        worksheet.Cell(1, 6).Value = "Password Expiry Date";
                        worksheet.Cell(1, 7).Value = "Created By";
                        worksheet.Cell(1, 8).Value = "Created Date";
                        worksheet.Cell(1, 9).Value = "Status";
                        // ... Add more headers if needed

                        // Format header row
                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                        // ... Apply additional formatting as needed

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1; // Serial Number
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].EmployeeName;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].EmployeeNumber;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].Username;
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].RoleName;
                            worksheet.Cell(i + 2, 6).Value = objData.returnData[i].PasswordExpirydate;
                            worksheet.Cell(i + 2, 7).Value = objData.returnData[i].CreatedUsername;
                            worksheet.Cell(i + 2, 8).Value = objData.returnData[i].CreatedDate;
                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 9).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 9).Value = "Inactive";
                            }



                            // ... Add more data cells if needed
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.Position = 0;
                            byte[] fileBytes = stream.ToArray();
                            retModel.returnData = GenericUtilities.SetReportData(fileBytes, ".xlsx");
                            retModel.transactionStatus = HttpStatusCode.OK;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public ResponseEntity<List<UserLoginLogViewModel>> GetLoginSessions(long? userId, DateTime? FromDate, DateTime? ToDate, long? RoleId)
        {
            var retModel = new ResponseEntity<List<UserLoginLogViewModel>>();
            try
            {
                var objLogs = _dbContext.UserSessions/*Where(c => c.LoggedInUserId == userId)*/
                    .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                    .Select(c => new
                    {
                        SessionInfo = c,
                        loggedinDate = c.LoginDateTime,
                        loggedOutDate = c.LogoutDateTime,
                        Username = c.User.UserName,
                        UserRole = c.User.Role.RoleName,
                        roleId = c.User.RoleId,
                    });

                objLogs = objLogs.OrderByDescending(i => i.loggedinDate);

                if (RoleId.HasValue)
                {
                    objLogs = objLogs.Where(i => i.roleId == RoleId);
                }

                if (FromDate.HasValue)
                {
                    objLogs = objLogs.Where(c => c.loggedinDate >= FromDate);
                }

                if (ToDate.HasValue)
                {
                    objLogs = objLogs.Where(c => c.loggedinDate <= ToDate);
                }

                objLogs = objLogs.OrderByDescending(c => c.SessionInfo.LoginDateTime);

                var objLogsQuery = objLogs.ToList();

                var objLogList = objLogsQuery.Select(c => new UserLoginLogViewModel()
                {
                    RoleName = c.UserRole,
                    UserName = c.Username,
                    LoginDateTime = c.SessionInfo.LoginDateTime,
                    SessionId = c.SessionInfo.SessionId,
                    LogoutDateTime = c.loggedOutDate != null ? c.loggedOutDate.Value : null,
                }).ToList();
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objLogList;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }
        public ResponseEntity<string> ExportUserSessionDatatoExcel(string search, long? Roleid, DateTime? FromDate, DateTime? ToDate)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetLoginSessions(0, FromDate, ToDate, Roleid);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Login Session");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Employee Name";
                        worksheet.Cell(1, 3).Value = "Employee Number";
                        worksheet.Cell(1, 4).Value = "Role";
                        worksheet.Cell(1, 5).Value = "LogIn Time";
                        worksheet.Cell(1, 6).Value = "LogOut Time";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].EmployeeName;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].EmployeeCode;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].RoleName;
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].LoginDateTime.ToString();
                            worksheet.Cell(i + 2, 6).Value = objData.returnData[i].LogoutDateTime.ToString();
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.Position = 0;
                            byte[] fileBytes = stream.ToArray();
                            retModel.returnData = GenericUtilities.SetReportData(fileBytes, ".xlsx");
                            retModel.transactionStatus = HttpStatusCode.OK;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
            }
            return retModel;
        }


        public async Task<bool> ValidateCurrentPasswordAsync(long? userId, string oldPassword)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return false;

            return user.Password == oldPassword;
        }

        public async Task<ResponseEntity<ChangePasswordViewModel>> ChangePassword(ChangePasswordViewModel model)
        {
            var retModel = new ResponseEntity<ChangePasswordViewModel>();

            try
            {

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.UserId == model.loggedinUserId);

                if (user == null)
                {
                    retModel.transactionStatus = HttpStatusCode.NotFound;
                    retModel.returnMessage = "User not found.";
                    return retModel;
                }


                if (user.Password != model.OldPassword)
                {
                    retModel.transactionStatus = HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Old password is incorrect.";
                    return retModel;
                }


                user.Password = model.NewPassword;
                user.PasswordExpiryDate = DateTime.UtcNow.AddMonths(3); // Example of setting a password expiry date

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnMessage = "Password changed successfully.";
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An error occurred while changing the password.";
            }

            return retModel;
        }

        public async Task<HomeViewModel> GetUserLoginInfoAsync(long userId)
        {
            var result = new HomeViewModel();

            // Get password expiry date from Users table
            var user = await _dbContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => new { u.PasswordExpiryDate })
                .SingleOrDefaultAsync();

            if (user != null)
            {
                result.PasswordExpiryDate = user.PasswordExpiryDate;
            }

            // Get the two latest login sessions (current and previous)
            var sessions = await _dbContext.UserSessions.Where(s => s.LoggedInUserId == userId)
                //.Where(s => s.LoggedInUserId == userId && s.Active)
                .OrderByDescending(s => s.LoginDateTime)
                .Take(2)
                .ToListAsync();

            if (sessions.Count > 0)
            {
                result.CurrentLogin = sessions[0].LoginDateTime;
                if (result.CurrentLogin != null && result.CurrentLogin != DateTime.MinValue)
                {
                    result.CurrentLoginDatetimeFormatted = GenericUtilities.ConvertAndFormatToKuwaitDateTime(result.CurrentLogin, GenericUtilities.dateTimeFormat);
                }
            }
            if (sessions.Count > 1)
            {
                result.PreviousLogin = sessions[1].LoginDateTime;

                if (result.PreviousLogin != null && result.PreviousLogin != DateTime.MinValue)
                {
                    result.PreviousLoginDatetimeFormatted = GenericUtilities.ConvertAndFormatToKuwaitDateTime(result.PreviousLogin, GenericUtilities.dateTimeFormat);
                }

            }
            return result;
        }

        public ResponseEntity<List<UserViewModel>> GetMemberDetails(string? Keyword, string? SearchText)
        {
            var retModel = new ResponseEntity<List<UserViewModel>>();

            try
            {
                IQueryable<Users> query = _dbContext.Users;

                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    string searchType = SearchText?.Trim().ToLower();

                    if (searchType == "username" || searchType == "all")
                    {
                        query = query.Where(u => u.UserName.Contains(Keyword));
                    }

                }
                var objModel = query.Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    Username = u.UserName,           // Assuming Username maps to Name

                }).ToList();


                // Assign to response entity
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }


    }
}
