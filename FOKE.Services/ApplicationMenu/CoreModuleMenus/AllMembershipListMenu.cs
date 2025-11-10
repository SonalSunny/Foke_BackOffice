using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class AllMembershipListMenu
    {
        public static List<AppMenu> GetAllMembershipListMenu()
        {
            return new List<AppMenu>()
            {

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.SearchMembership,
                    ParentMenuId = MenuMasterStructs.AllMemberssList,
                    MenuIcon = "",
                    MenuTitle = "Search Member",
                    MenuDescription = "Search Member",
                    Path = "SearchMember/MemberSearchForm",
                    PageCode = "Search Member",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.AllMemberssList,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-list",
                    MenuTitle = "MENU_MEMBERS_LIST",
                    MenuDescription = "Members List",
                    Path = "",
                    PageCode = "MembershipList Main Menu",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },  new AppMenu()
                {
                    MenuId = MenuMasterStructs.AllMemberss,
                    ParentMenuId = MenuMasterStructs.AllMemberssList,
                    MenuIcon = "",
                    MenuTitle = "MENU_ALL_MEMBERS_LIST",
                    MenuDescription = "All Members List" ,
                    Path = "AllMembersList/Index",
                    PageCode = "All MembershipList",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                 {
                    MenuId = MenuMasterStructs.MembershipByArea,
                    ParentMenuId = MenuMasterStructs.AllMemberssList,
                    MenuIcon = "",
                    MenuTitle = "MENU_MEMBERS_BY_AREA",
                    MenuDescription = "Members By Area",
                    Path = "MembersList/MembersByArea/Index",
                    PageCode = "Members By Area",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                }
                //new AppMenu()
                // {
                //    MenuId = MenuMasterStructs.MembershipByZone,
                //    ParentMenuId = MenuMasterStructs.AllMemberssList,
                //    MenuIcon = "",
                //    MenuTitle = "MENU_MEMBERS_BY_ZONE",
                //    MenuDescription = "Members By Zone",
                //    Path = "MembersList/MembersByZone/Index",
                //    PageCode = "Members By Zone",
                //    DisplayOrder = 1,
                //    GroupBy="Settings",
                //    MenuClaims= new List<MenuClaim>() {
                //        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                //    }
                //},new AppMenu()
                // {
                //    MenuId = MenuMasterStructs.MembershipByUnit,
                //    ParentMenuId = MenuMasterStructs.AllMemberssList,
                //    MenuIcon = "",
                //    MenuTitle = "MENU_MEMBERS_BY_UNIT",
                //    MenuDescription = "Members By Unit",
                //    Path = "MembersList/MembersByUnit/Index",
                //    PageCode = "Members By Unit",
                //    DisplayOrder = 1,
                //    GroupBy="Settings",
                //    MenuClaims= new List<MenuClaim>() {
                //        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                //    }
                //}
            };
        }

    }
}
