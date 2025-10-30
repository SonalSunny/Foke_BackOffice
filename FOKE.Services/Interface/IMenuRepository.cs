using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.MenuManagement.DTO;
using System.Security.Claims;

namespace FOKE.Services.Interface
{
    public interface IMenuRepository
    {
        Task<List<MenuRole>> GetApplicationMenusBygroup(ClaimsPrincipal principal);
        Task<RoleAdministrationViewModel> GetPermissionsByRoleIdAsync(string roleCode, long? roleId);
        Task<ResponseEntity<RoleAdministrationViewModel>> SaveRoleAdministrations(RoleAdministrationViewModel roleAdministration, long roleId);
    }
}
