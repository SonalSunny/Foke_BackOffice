using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class NotificationMenu
    {
        public static List<AppMenu> GetNotificationMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.Notification,
                    ParentMenuId = null,
                    MenuIcon = "sidebar-item-icon fa fa-bell",
                    MenuTitle = "MENU_NOTIFICATION",
                    MenuDescription = "Notification",
                    Path = "",
                    PageCode = "Notification",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.ComposeNotification,
                    ParentMenuId = MenuMasterStructs.Notification,
                    MenuIcon = "",
                    MenuTitle = "MENU_COMPOSE",
                    MenuDescription = "Compose Notification" ,

                    Path = "Notifications/Compose/Index",
                    PageCode = "Compose Notification",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.ViewNotification,
                    ParentMenuId = MenuMasterStructs.Notification,
                    MenuIcon = "",
                    MenuTitle = "MENU_SENT_ITEMS",
                    MenuDescription = "SendItems" ,

                    Path = "Notifications/SentItems/Index",
                    PageCode = "SendItems",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MessagesFromDigitalId,
                    ParentMenuId = MenuMasterStructs.Notification,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_MESSAGES_FROM_DIGITALID",
                    MenuDescription = "Messages From DigitalID",
                    Path = "ClientEnquiery/Index",
                    PageCode = "Messages From DigitalID",
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
