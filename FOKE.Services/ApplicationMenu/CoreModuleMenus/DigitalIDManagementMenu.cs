using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class DigitalIDManagementMenu
    {

        public static List<AppMenu> GetDigitalIDManagementMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.DigitalIDManagement,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-id-card",
                    MenuTitle = "DigitalID Management",
                    MenuDescription = "DigitalID Management",
                    Path = "DigitalIDManagement/Index",
                    PageCode = "DigitalID Management",
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
