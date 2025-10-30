using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class SearchMembershipMenu
    {
        public static List<AppMenu> GetSearchMembershipMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.SearchMembership,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-search",
                    MenuTitle = "Search Member",
                    MenuDescription = "Search Member",
                    Path = "SearchMember/MemberSearchForm",
                    PageCode = "Search Membership",
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
