using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class DashboardMenu
    {
        public static List<AppMenu> GetDashBoardMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.Dashboard,
                    ParentMenuId = null,
                    MenuIcon = "fa fa-dashboard",
                    MenuTitle = "MENU_DASHBOARD",
                    MenuDescription = "Dashboard",
                    Path = "",
                    PageCode = "Dashboard",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },  new AppMenu()
                {
                    MenuId = MenuMasterStructs.NewMembers,
                    ParentMenuId = MenuMasterStructs.Dashboard,
                    MenuIcon = "",
                    MenuTitle = "MENU_MEMBERS",
                    MenuDescription = "Members" ,
                    Path = "Dashboards/ByArea/Index",
                    PageCode = "By Area",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.FeeCollectionReport,
                    ParentMenuId = MenuMasterStructs.Dashboard,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_MEMBERSHIP_FEE",
                    MenuDescription = "Fee Collection Report",
                    Path = "FeeCollectionReport/FeeCollection",
                    PageCode = "Fee Collection Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                }
            };
        }
    }
}
