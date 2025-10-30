using FOKE.Entity.MenuManagement.DTO;
using FOKE.Services.ApplicationMenu.CoreModuleMenus;

namespace FOKE.Services.ApplicationMenu
{
    public static class AplicationMenuBase
    {

        public static List<MenuRole> GetApplicationMenus()
        {
            var objBaseList = new List<MenuRole>();
            objBaseList.Add(new MenuRole() { RoleCode = "FOKE", DisplayOrder = 1, MenuTitle = "ERP", MenuIcon = "<i class=\"fas fa-user-cog\"></i>", MenuGroups = GetFOKEMenus() });
            return objBaseList;
        }
        public static List<MenuGroup> GetFOKEMenus()
        {
            var objBaseList = new List<MenuGroup>();

            var objMMenu1 = new MenuGroup();
            objMMenu1.DisplayOrder = 1;
            objMMenu1.GroupTitle = "";
            objMMenu1.Menus = new List<AppMenu>();
            objMMenu1.Menus.AddRange(HomeMenu.GetHomeMenu());
            objMMenu1.Menus.AddRange(DashboardMenu.GetDashBoardMenu());
            objMMenu1.Menus.AddRange(NotificationMenu.GetNotificationMenu());
            objMMenu1.Menus.AddRange(IssueMembershipMenu.GetIssueMembershipMenu());
            objMMenu1.Menus.AddRange(AllMembershipListMenu.GetAllMembershipListMenu());
            objMMenu1.Menus.AddRange(MembershipActionMenu.GetCancelMembershipMenu());
            objMMenu1.Menus.AddRange(FileManagerMenu.GetAssociationMenu());
            objMMenu1.Menus.AddRange(MasterMenus.GetMasterMenu());
            objMMenu1.Menus.AddRange(ReportAndAnalysisMenus.GetReportAndAnalysisMenu());
            objMMenu1.Menus.AddRange(ConfiguarationMenus.GetConfiguarationMenu());



            objBaseList.Add(objMMenu1);
            return objBaseList;

        }
    }
}
