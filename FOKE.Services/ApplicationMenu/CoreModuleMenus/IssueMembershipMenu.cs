using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class IssueMembershipMenu
    {
        public static List<AppMenu> GetIssueMembershipMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.IssueMembership,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-user-plus",
                    MenuTitle = "Issue Membership",
                    MenuDescription = "Issue Membership",
                    Path = "IssueMembership/Index",
                    PageCode = "Issue Membership",
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
