using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class CommitteManagementMenu
    {
        public static List<AppMenu> GetCommitteManagementMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.CommitteManagement,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-users-cog",
                    MenuTitle = "Committee Management",
                    MenuDescription = "Committee Management",
                    Path = "CommitteManagement/Index",
                    PageCode = "Committe Management",
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
