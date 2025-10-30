using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Services.ApplicationMenu.CoreModuleMenus
{
    public static class MasterMenus
    {
        public static List<AppMenu> GetMasterMenu()
        {
            return new List<AppMenu>()
            {
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.DataEntryMaster,
                    ParentMenuId = null,
                    MenuIcon = "sidebar-item-icon fa fa-database",
                    MenuTitle = "MENU_MASTER",
                    MenuDescription = "DataEntryMaster",
                    Path = "#",
                    PageCode = "DataEntryMaster",
                    DisplayOrder = 1,
                    GroupBy="Settings",
                    MenuClaims= new List<MenuClaim>() {
                        new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription }

                    }
                },
                new AppMenu()
                {
                    MenuId = MenuMasterStructs.UserMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_USER_MASTER",
                    MenuDescription = "UserMaster",
                    Path = "User/Index",
                    PageCode = "DataEntryMaster_UserMaster",
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
                    MenuId = MenuMasterStructs.UserRolesMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_ROLES",
                    MenuDescription = "UserRolesMaster",
                    Path = "Role/Index",
                    PageCode = "DataEntryMaster_UserRolesMaster",
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
                    MenuId = MenuMasterStructs.LookUp,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_LOOKUPMASTER",
                    MenuDescription = "LookUp Master",
                    Path = "LookupMaster/Index",
                    PageCode = "DataEntryMaster_LookupMaster",
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
                    MenuId = MenuMasterStructs.AreaMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_AREA_MASTER",
                    MenuDescription = "Area Master",
                    Path = "Area/Index",
                    PageCode = "DataEntryMaster_AreaMaster",
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
                    MenuId = MenuMasterStructs.ZoneMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_ZONE_MASTER",
                    MenuDescription = "Zone Master",
                    Path = "Zone/Index",
                    PageCode = "DataEntryMaster_ZoneMaster",
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
                    MenuId = MenuMasterStructs.UnitMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_UNIT_MASTER",
                    MenuDescription = "Unit Master",
                    Path = "Unit/Index",
                    PageCode = "DataEntryMaster_Unit Master",
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
                    MenuId = MenuMasterStructs.ProfessionMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_PROFESSION_MASTER",
                    MenuDescription = "Profession Master",
                    Path = "Profession/Index",
                    PageCode = "DataEntryMaster_Profession Master",
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
                    MenuId = MenuMasterStructs.WorkPlaceMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_WORKPLACE",
                    MenuDescription = "WorkPlace Master",
                    Path = "WorkPlace/Index",
                    PageCode = "DataEntryMaster_WorkPlace Master",
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
                    MenuId = MenuMasterStructs.FileMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_FILE_MASTER",
                    MenuDescription = "File Master",
                    Path = "FileMaster/Index",
                    PageCode = "DataEntryMaster_File Master",
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
                    MenuId = MenuMasterStructs.FolderMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_FOLDER_MASTER",
                    MenuDescription = "Folder Master",
                    Path = "FolderMaster/Index",
                    PageCode = "DataEntryMaster_Folder Master",
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
                    MenuId = MenuMasterStructs.DepartmentMaster,
                    ParentMenuId = MenuMasterStructs.DataEntryMaster,
                    MenuIcon = "sidebar-item-icon fa fa-th-large",
                    MenuTitle = "MENU_DEPARTMENT_MASTER",
                    MenuDescription = "Department Master",
                    Path = "Department/Index",
                    PageCode = "DataEntryMaster_Department Master",
                    DisplayOrder = 1,
                    MenuClaims= new List<MenuClaim>() {
                    new MenuClaim() { ClaimType = ClaimStructs.ViewCode, ClaimName = ClaimStructs.ViewDescription },
                        new MenuClaim() { ClaimType = ClaimStructs.AddCode, ClaimName = ClaimStructs.AddDescription },
                        new MenuClaim() { ClaimType = ClaimStructs.EditCode, ClaimName = ClaimStructs.EditDescription },
                        new MenuClaim() { ClaimType = ClaimStructs.DeleteCode, ClaimName = ClaimStructs.DeleteDescription }
                    }
                }
            };
        }
    }
}
