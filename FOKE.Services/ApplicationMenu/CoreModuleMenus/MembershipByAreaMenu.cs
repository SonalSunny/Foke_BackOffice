using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class MembershipByAreaMenu
    {
        public static List<AppMenu> GetMembershipByAreaMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MembershipByArea,
                    ParentMenuId = null,
                    MenuIcon = "fa-globe",
                    MenuTitle = "Membership By Area",
                    MenuDescription = "Membership By Area",
                    Path = "",
                    PageCode = "Membership By Area",
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
