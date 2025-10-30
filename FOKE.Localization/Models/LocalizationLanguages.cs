namespace FOKE.Localization.Models
{
    public class LocalizationLanguages
    {
        public List<Language> Languages { get; }
        public LocalizationLanguages()
        {
            Languages = new List<Language>();
            Languages.Add(new Language { Name = "English", Culture = "en-US", Direction = "rtl" });
            Languages.Add(new Language { Name = "Arabic", Culture = "ar-AE", Direction = "ltl" });
        }
    }
    public class ResourceModules
    {
        public List<ResourceModule> Modules { get; }

        public ResourceModules()
        {
            Modules = new List<ResourceModule>();
            Modules.Add(new ResourceModule { ModuleCode = "Menu", ModuleName = "Menu", ResourceFile = "MenuResource" });
            Modules.Add(new ResourceModule { ModuleCode = "IssueMembership", ModuleName = "IssueMembership", ResourceFile = "IssueMemberResource" });
            Modules.Add(new ResourceModule { ModuleCode = "Notification", ModuleName = "Notification", ResourceFile = "NotificationResource" });
            Modules.Add(new ResourceModule { ModuleCode = "MembershipActions", ModuleName = "MembershipActions", ResourceFile = "MembershipActionResource" });
            Modules.Add(new ResourceModule { ModuleCode = "MasterData", ModuleName = "MasterData", ResourceFile = "MasterDataResource" });

            Modules.Add(new ResourceModule { ModuleCode = "Configuration", ModuleName = "Configuration", ResourceFile = "ConfigurationResource" });
            Modules.Add(new ResourceModule { ModuleCode = "Home", ModuleName = "Home", ResourceFile = "HomeResource" });
            Modules.Add(new ResourceModule { ModuleCode = "Welcome", ModuleName = "Welcome", ResourceFile = "WelcomeResource" });
            Modules.Add(new ResourceModule { ModuleCode = "Shared", ModuleName = "Shared", ResourceFile = "SharedResource" });
            Modules.Add(new ResourceModule { ModuleCode = "UserMaster", ModuleName = "User Master", ResourceFile = "UserResource" });
            Modules.Add(new ResourceModule { ModuleCode = "RoleMaster", ModuleName = "User Roles Master", ResourceFile = "RoleResource" });
            Modules.Add(new ResourceModule { ModuleCode = "LookUpMaster", ModuleName = "LookUp Master", ResourceFile = "LookupResource" });
            Modules.Add(new ResourceModule { ModuleCode = "ProjectConfiguration", ModuleName = "Project Configuration", ResourceFile = "ProjectConfigurationResources" });
            Modules.Add(new ResourceModule { ModuleCode = "Association", ModuleName = "Association", ResourceFile = "AssociationResource" });
            Modules.Add(new ResourceModule { ModuleCode = "Notification", ModuleName = "Notification", ResourceFile = "NotificationResource" });
        }
    }
}