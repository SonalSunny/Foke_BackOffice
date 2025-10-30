using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class MembershipActionMenu
    {
        public static List<AppMenu> GetCancelMembershipMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.PayMembershipFee,
                    ParentMenuId = MenuMasterStructs.MembershipActions,
                    MenuIcon = "fa-credit-card",
                    MenuTitle = "MENU_PAY_MEMBERSHIP_FEE",
                    MenuDescription = "Pay Memberhip Fee",
                    Path = "PayMembershipFee/MemberSearchForm",
                    PageCode = "Pay Membership Fee",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MembershipActions,
                    ParentMenuId = null,
                    MenuIcon = "fas fa-user-cog",
                    MenuTitle = "MENU_MEMBERSHIP_ACTIONS",
                    MenuDescription = "Membership Actions",
                    Path = "",
                    PageCode = "Cancel Membership",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.EditMembership,
                    ParentMenuId = MenuMasterStructs.MembershipActions,
                    MenuIcon = "fas fa-user-edit",
                    MenuTitle = "MENU_EDIT_MEMBERSHIP",
                    MenuDescription = "Edit Membership",
                    Path = "EditMembership/MemberSearchForm",
                    PageCode = "Edit Membership",
                    DisplayOrder = 1,
                    MenuClaims= new List<MenuClaim>() {
                    new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription },
                        new MenuClaim() { ClaimType = ClaimStructs.AddCode, ClaimName = ClaimStructs.AddDescription },
                        new MenuClaim() { ClaimType = ClaimStructs.EditCode, ClaimName = ClaimStructs.EditDescription },
                        new MenuClaim() { ClaimType = ClaimStructs.DeleteCode, ClaimName = ClaimStructs.DeleteDescription }
                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.CancelMembership,
                    ParentMenuId = MenuMasterStructs.MembershipActions,
                    MenuIcon = "fas fa-user-slash",
                    MenuTitle = "MENU_CANCEL_MEMBERSHIP",
                    MenuDescription = "Cancel Membership",
                    Path = "MembershipCancelation/MemberSearchForm",
                    PageCode = "Cancel Membership",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                } ,
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.DigitalIDManagement,
                    ParentMenuId = MenuMasterStructs.MembershipActions,
                    MenuIcon = "fas fa-id-card",
                    MenuTitle = "MENU_DIGITALID_MANAGEMENT",
                    MenuDescription = "DigitalID Management",
                    Path = "DigitalIDManagement/Index",
                    PageCode = "DigitalID Management",
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
