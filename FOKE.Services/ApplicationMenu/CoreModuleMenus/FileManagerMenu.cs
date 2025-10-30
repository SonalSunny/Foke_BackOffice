using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class FileManagerMenu
    {
        public static List<AppMenu> GetAssociationMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                 {
                     MenuId = MenuMasterStructs.Campaigns,
                     ParentMenuId = MenuMasterStructs.MembershipActions,
                     MenuIcon = "fa-credit-card",
                     MenuTitle = "MENU_CAMPAIGNS",
                     MenuDescription = "Campaigns",
                     Path = "FeeCampaign/Index",
                     PageCode = "Campaigns",
                     DisplayOrder = 1,
                     GroupBy="Settings",
                     MenuClaims= new List<MenuClaim>() {
                         new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                     }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.Association,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-users",
                    MenuTitle = "MENU_ASSOCIATION",
                    MenuDescription = "Association",
                    Path = "",
                    PageCode = "Association",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.CommitteManagement,
                    ParentMenuId = MenuMasterStructs.Association,
                    MenuIcon = "fas fa-users-cog",
                    MenuTitle = "MENU_COMMITTEE_MANAGEMENT",
                    MenuDescription = "Committee Management",
                    Path = "CommitteManagement/Index",
                    PageCode = "Committe Management",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.news,
                    ParentMenuId = MenuMasterStructs.Association,
                    MenuIcon = "fa fa-folder",
                    MenuTitle = "MENU_NEWS_AND_EVENTS",
                    MenuDescription = "News and Events",
                    Path = "NewsAndEvents/Index",
                    PageCode = "News and Events",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.OfferstoActiveMembers,
                    ParentMenuId = MenuMasterStructs.Association,
                    MenuIcon = "fas fa-gift",
                    MenuTitle = "MENU_OFFERS",
                    MenuDescription = "Offers To Active Members",
                    Path = "Offers/Index",
                    PageCode = "Offers To Active Members",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.Sponsorship,
                    ParentMenuId = MenuMasterStructs.Association,
                    MenuIcon = "fas fa-gift",
                    MenuTitle = "MENU_SPONSORSHIP",
                    MenuDescription = "Sponsorship",
                    Path = "Sponsorship/Index ",
                    PageCode = "Sponsorship",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.Accounts,
                    ParentMenuId = MenuMasterStructs.Association,
                    MenuIcon = "fas fa-gift",
                    MenuTitle = "MENU_ACCOUNTS",
                    MenuDescription = "Accounts",
                    Path = "AccountData/Index",
                    PageCode = "Accounts",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.FileManagerMenu,
                    ParentMenuId = MenuMasterStructs.Association,
                    MenuIcon = "fa fa-folder",
                    MenuTitle = "MENU_FILE_MANAGER",
                    MenuDescription = "File Manager",
                    Path = "File_Manager/Index?pageCode=C006",
                    PageCode = "File Manager",
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
