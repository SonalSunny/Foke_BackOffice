using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class NewsAndEventsMenu
    {
        public static List<AppMenu> GetNewsAndEventsMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.NewsandEvent,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-bullhorn",
                    MenuTitle = "News and Events",
                    MenuDescription = "News and Events",
                    Path = "NewsAndEvents/Index",
                    PageCode = "News and Events",
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
