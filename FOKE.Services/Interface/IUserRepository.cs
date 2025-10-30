using FOKE.Entity;
using FOKE.Entity.ChangePassword.ViewModel;
using FOKE.Entity.Identity.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IUserRepository
    {
        Task<ResponseEntity<UserViewModel>> RegisterUser(UserViewModel model);
        ResponseEntity<List<UserViewModel>> GetAllUsers(long? Status, long? Roleid, DateTime? PasswordExpirydateFrom, DateTime? PasswordExpirydateTo);
        ResponseEntity<bool> DeleteUser(UserViewModel objModel);
        ResponseEntity<UserViewModel> GetUserbyId(long userId);
        Task<ResponseEntity<UserViewModel>> UpdateUser(UserViewModel model);
        ResponseEntity<string> ExportUserDatatoExcel(string search, long? statusid, long? Roleid, DateTime? PasswordExpirydateFrom, DateTime? PasswordExpirydateTo);
        ResponseEntity<List<UserLoginLogViewModel>> GetLoginSessions(long? userId, DateTime? FromDate, DateTime? ToDate, long? RoleId);
        ResponseEntity<string> ExportUserSessionDatatoExcel(string search, long? Roleid, DateTime? FromDate, DateTime? ToDate);
        Task<bool> ValidateCurrentPasswordAsync(long? userId, string oldPassword);

        Task<ResponseEntity<ChangePasswordViewModel>> ChangePassword(ChangePasswordViewModel model);

        Task<HomeViewModel> GetUserLoginInfoAsync(long userId);
        ResponseEntity<List<UserViewModel>> GetMemberDetails(string? Keyword, string? SearchText);
    }
}
