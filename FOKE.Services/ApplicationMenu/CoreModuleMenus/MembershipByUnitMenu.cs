using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class MembershipByUnitMenu
    {
        public static List<AppMenu> GetMembershipByUnitMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MembershipByUnit,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-building",
                    MenuTitle = "Membership By Unit",
                    MenuDescription = "Membership By Unit",
                    Path = "",
                    PageCode = "Membership By Unit",
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
