using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class MembershipByZoneMenu
    {
        public static List<AppMenu> GetMembershipByZoneMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MembershipByZone,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-map-marked-alt",
                    MenuTitle = "Membership By Zone",
                    MenuDescription = "Membership By Zone",
                    Path = "",
                    PageCode = "Membership By Zone",
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
