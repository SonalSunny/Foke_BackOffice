using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IUserLoginRepository
    {
        Task<ResponseEntity<UserViewModel>> Login(LoginViewModel model);
        Task<bool> Logout();
    }
}
