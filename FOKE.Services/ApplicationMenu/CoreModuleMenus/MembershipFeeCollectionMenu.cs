using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class MembershipFeeCollectionMenu
    {
        public static List<AppMenu> GetMembershipFeeCollectionMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MembershipFeeCollection,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-credit-card",
                    MenuTitle = "Membership Fee",
                    MenuDescription = "Membership Fee",
                    Path = "",
                    PageCode = "Membership Fee",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                }, new AppMenu()
                {
                    MenuId = MenuMasterStructs.Campaigns,
                    ParentMenuId = MenuMasterStructs.MembershipFeeCollection,
                    MenuIcon = "fa-credit-card",
                    MenuTitle = "Campaigns",
                    MenuDescription = "Campaigns",
                    Path = "FeeCampaign/Index",
                    PageCode = "Campaigns",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                }, new AppMenu()
                {
                    MenuId = MenuMasterStructs.PayMembershipFee,
                    ParentMenuId = MenuMasterStructs.MembershipFeeCollection,
                    MenuIcon = "fa-credit-card",
                    MenuTitle = "Pay Membership Fee",
                    MenuDescription = "Pay Membership Fee",
                    Path = "PayMembershipFee/Index",
                    PageCode = "Pay Membership Fee",
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
