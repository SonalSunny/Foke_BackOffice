using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class OffersToActiveMembersMenu
    {
        public static List<AppMenu> GetOffersToActiveMembersMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.OfferstoActiveMembers,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-gift",
                    MenuTitle = "Offers To Active Members",
                    MenuDescription = "Offers To Active Members",
                    Path = "Offers/Index",
                    PageCode = "Offers To Active Members",
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
