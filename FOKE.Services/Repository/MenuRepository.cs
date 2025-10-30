using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.MenuManagement.DTO;
using FOKE.Entity.RoleData.DTO;
using FOKE.Services.ApplicationMenu;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IMemoryCache _cache;
        private readonly FOKEDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;


        public MenuRepository(FOKEDBContext context, IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

            try
            {
                claimsPrincipal = _httpContextAccessor?.HttpContext?.User as ClaimsPrincipal;
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

        public async Task<List<MenuRole>> GetApplicationMenusBygroup(ClaimsPrincipal principal)
        {
            var menu = new List<MenuRole>();
            var isAuthenticated = principal.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return new List<MenuRole>();
            }
            var userId = GetUserId(principal);
            var usrInfo = _context.Users.Where(c => c.UserId == Convert.ToInt64(userId)).FirstOrDefault();
            if (usrInfo != null)
            {
                //if (usrInfo.IsAdminUser)
                //{
                //    usrInfo.RoleCode = "SuperAdmin";
                //}

                var appMenus = AplicationMenuBase.GetApplicationMenus();
                bool isAdmin = _context.Users.Any(x => x.UserId == Convert.ToInt64(userId) && x.RoleId == 1);
                if (isAdmin)
                {
                    appMenus = appMenus.OrderBy(c => c.DisplayOrder).ToList();
                    menu = appMenus;
                }
                if (isAdmin == false && appMenus != null)
                {
                    var roleMnu = new MenuRole();
                    var objRoleClaim = await _context.RoleGroupClaims.Where(c => c.ClaimType == "View" && c.ClaimValue == true && c.RoleId == usrInfo.RoleId).ToListAsync();
                    var roleMenu = appMenus.Where(c => c.RoleCode == "FOKE").FirstOrDefault();
                    if (roleMenu != null && roleMenu.RoleCode != null)
                    {
                        roleMnu = roleMenu.ShallowCopy();
                        roleMnu.MenuGroups = new List<MenuGroup>();
                        foreach (var objGroup in roleMenu.MenuGroups)
                        {

                            var mnuGrp = new MenuGroup();
                            mnuGrp.GroupTitle = objGroup.GroupTitle;
                            mnuGrp.DisplayOrder = objGroup.DisplayOrder;
                            mnuGrp.Menus = new List<AppMenu>();
                            foreach (var mnu in objGroup.Menus.Where(c => c.ParentMenuId == null).OrderBy(c => c.DisplayOrder))
                            {
                                if (objRoleClaim.Any(c => c.PageCode == mnu.PageCode))
                                {

                                    mnuGrp.Menus.Add(mnu);

                                    var childMenu = objGroup.Menus.Where(c => c.ParentMenuId == mnu.MenuId).OrderBy(c => c.DisplayOrder)?.ToList();
                                    if (childMenu != null)
                                    {
                                        foreach (var cMnu in childMenu)
                                        {
                                            if (objRoleClaim.Any(c => c.PageCode == cMnu.PageCode))
                                            {
                                                mnuGrp.Menus.Add(cMnu);
                                            }
                                        }
                                    }
                                }
                            }
                            roleMnu.MenuGroups.Add(mnuGrp);
                        }
                        menu.Add(roleMnu);
                    }
                }
            }

            return menu;
        }
        private string GetUserId(ClaimsPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Name)?.Value;
        }
        public async Task<RoleAdministrationViewModel> GetPermissionsByRoleIdAsync(string roleCode, long? roleId)
        {
            var roleAdministration = new RoleAdministrationViewModel();
            try
            {
                var appRoleMenus = AplicationMenuBase.GetApplicationMenus();
                //if (roleCode != null)
                //{
                var appMenus = appRoleMenus.Where(c => c.RoleCode == roleCode).FirstOrDefault();
                roleAdministration.RoleCode = roleCode;
                roleAdministration.RoleId = roleId;
                var objRole = await _context.Roles.Include(c => c.RoleGroupClaims).Where(c => c.RoleId == roleId).FirstOrDefaultAsync();
                if (objRole != null && appMenus.MenuGroups != null)
                {
                    roleAdministration.RoleName = objRole.RoleName ?? "";
                    roleAdministration.RoleId = objRole.RoleId;

                    foreach (var objGroup in appMenus.MenuGroups)
                    {
                        foreach (var mnu in objGroup.Menus)
                        {
                            var showMenuPolicy = mnu.MenuClaims.Where(c => c.ClaimType.StartsWith("View")).FirstOrDefault();
                            var privilegePolicy = mnu.MenuClaims.Where(c => c.ClaimType != showMenuPolicy?.ClaimType)?.ToList();
                            var mnuGrpPermission = objRole?.RoleGroupClaims?.Where(c => c.ClaimType == showMenuPolicy?.ClaimType && c.PageCode == mnu.PageCode).FirstOrDefault();
                            mnu.Selected = mnuGrpPermission?.ClaimValue == true ? true : false;

                            foreach (var policy in privilegePolicy)
                            {
                                var mnuPolicyPermission = objRole?.RoleGroupClaims?.Where(c => c.ClaimType == policy?.ClaimType).FirstOrDefault();
                                policy.Selected = mnuPolicyPermission?.ClaimValue == true ? true : false;
                            }
                        }
                    }
                    roleAdministration.MenuGroups = appMenus.MenuGroups;
                }
                else
                {
                    roleAdministration.MenuGroups = new List<MenuGroup>();
                }
                // }
            }
            catch (Exception ex)
            {

            }

            return roleAdministration;
        }
        public async Task<ResponseEntity<RoleAdministrationViewModel>> SaveRoleAdministrations(RoleAdministrationViewModel roleAdministration, long roleId)
        {
            var objResponce = new ResponseEntity<RoleAdministrationViewModel>();

            if (roleAdministration != null)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    //&& p.Active == true && p.Role.Active == true
                    var userPermission = _context.RoleGroupClaims.Where(p => p.RoleId == roleId);
                    objResponce.returnData = new RoleAdministrationViewModel();

                    _context.RoleGroupClaims.RemoveRange(userPermission);

                    var objRoleClaims = new List<RoleGroupClaim>();
                    foreach (var objGroup in roleAdministration?.MenuGroups)
                    {
                        foreach (var objMenu in objGroup?.Menus)
                        {
                            var objRoleClaim = new RoleGroupClaim();

                            objRoleClaim.RoleId = roleId;
                            objRoleClaim.ClaimType = ClaimStructs.ViewCode;
                            objRoleClaim.PageCode = objMenu.PageCode;
                            objRoleClaim.ClaimValue = objMenu.Selected;
                            objRoleClaim.Active = true;
                            objRoleClaims.Add(objRoleClaim);

                            if (objMenu.MenuClaims?.Count() > 0)
                            {
                                foreach (var objClaim in objMenu.MenuClaims)
                                {
                                    if (objClaim.ClaimType != ClaimStructs.ViewCode)
                                    {
                                        if (objMenu.Selected != true)
                                        {
                                            objClaim.Selected = false;
                                        }
                                        objRoleClaim = new RoleGroupClaim();
                                        objRoleClaim.RoleId = roleId;
                                        objRoleClaim.ClaimType = objClaim.ClaimType;
                                        objRoleClaim.PageCode = objMenu.PageCode;
                                        objRoleClaim.ClaimValue = objClaim.Selected;
                                        objRoleClaim.Active = true;
                                        objRoleClaims.Add(objRoleClaim);
                                    }
                                }
                            }
                        }
                    }

                    await _context.RoleGroupClaims.AddRangeAsync(objRoleClaims);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _cache.Remove("RolePermissions");
                    objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                    objResponce.returnMessage = "Saved Successfully";
                    objResponce.returnData = await GetPermissionsByRoleIdAsync(roleAdministration.RoleCode, roleId);
                }
                catch (Exception ex)
                {
                    objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    objResponce.returnMessage = "Interval Server Error Occurer";
                    await transaction.RollbackAsync();
                    //_Logger.LogError(ex.Message, ex);
                }

            }
            else
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                objResponce.returnMessage = "Failed to Save Role Permission";
            }
            return objResponce;
        }



    }
}
