using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class ReportAndAnalysisMenus
    {
        public static List<AppMenu> GetReportAndAnalysisMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.ReportAndAnalysis,
                    ParentMenuId = null,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_REPORT_AND_ANALYSIS",
                    MenuDescription = "Report and Analysis",
                    Path = "#",
                    PageCode = "Report and Analysis",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                }, new AppMenu()
                {
                    MenuId = MenuMasterStructs.UserLoginLog,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_USER_LOGIN_LOG",
                    MenuDescription = "User Login Log",
                    Path = "UserLoginLog/Index",
                    PageCode = "User Login Log",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.AllMembersList,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_ALL_MEMBERS_REPORT",
                    MenuDescription = "All Members Report",
                    Path = "AllMembersList/Index?PageCode=Report",
                    PageCode = "All Members Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.ActiveMembersList,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_ACTIVE_MEMBERS_REPORT",
                    MenuDescription = "Active Members Report",
                    Path = "ActiveMembersList/Index",
                    PageCode = "Active Members Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.ExpiredMembersList,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_UNPAID_MEMBERS_REPORT",
                    MenuDescription = "Unpaid Members Report",
                    Path = "ExpiredMembersList/Index",
                    PageCode = "Unpaid Members Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.RejectedMembersList,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_REJECTED_APPLICANTS_REPORT",
                    MenuDescription = "Rejected Applicants Report",
                    Path = "RejectedMembersList/Index",
                    PageCode = "Rejected Members Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.CancelledMembersList,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_CANCELLED_MEMBERS_REPORT",
                    MenuDescription = "Cancelled Members Report",
                    Path = "CancelledMembersList/Index",
                    PageCode = "Cancelled Members Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.FeeCollectionSummary,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_FEE_COLLECTION_SUMMARY_REPORT",
                    MenuDescription = "Fee Collection Summary Report",
                    Path = "PaymentReports/SummaryReport/Index",
                    PageCode = "Fee Collection Summary Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.FeeCollectionDetail,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_FEE_COLLECTION_DETAIL_REPORT",
                    MenuDescription = "Fee Collection Detail Report",
                    Path = "PaymentReports/DetailReport/Index",
                    PageCode = "Fee Collection Detail Report",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                     
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.MembersByRealExpiry,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_MEMBERS_BY_REAL_EXPIRY",
                    MenuDescription = "Members By Real Expiry",
                    Path = "PaymentReports/MembersByRealExpiry/Index",
                    PageCode = "Members By Real Expiry",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },

                new AppMenu()
                {
                    MenuId = MenuMasterStructs.Membership_Report,
                    ParentMenuId = MenuMasterStructs.ReportAndAnalysis,
                    MenuIcon = "sidebar-item-icon fa fa-cogs",
                    MenuTitle = "MENU_MEMBERSHIP_REPORT",
                    MenuDescription = "Membership Report",
                    Path = "PaymentReports/Membership_Report/Index",
                    PageCode = "Membership Report",
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

