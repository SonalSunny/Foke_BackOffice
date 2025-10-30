using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class AppManagementMenu
    {
        public static List<AppMenu> GetAppManagementMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.AppManagement,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-tools",
                    MenuTitle = "App Management",
                    MenuDescription = "App Management",
                    Path = "",
                    PageCode = "App Management",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

            };
        }
    }
}
