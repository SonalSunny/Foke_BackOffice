namespace FOKE.Entity.MenuManagement.DTO
{
    public class AppMenu
    {
        public bool Selected { get; set; }
        public long MenuId { get; set; }
        public long? ParentMenuId { get; set; }
        public string Separator { get; set; }
        public string SeparatorIcon { get; set; }
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; }
        public string MenuDescription { get; set; }
        public string Path { get; set; }
        public string PageCode { get; set; }
        public int DisplayOrder { get; set; }
        public string GroupBy { get; set; }
        public bool HasMenuDataCount { get; set; }
        public List<MenuClaim> MenuClaims { get; set; }
    }
    public class MenuGroup
    {
        public string GroupTitle { get; set; }
        public int DisplayOrder { get; set; }
        public List<AppMenu> Menus { get; set; }

    }

    public class MenuRole
    {
        public string RoleCode { get; set; }
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; }
        public int DisplayOrder { get; set; }
        public List<MenuGroup> MenuGroups { get; set; }
        public MenuRole ShallowCopy()
        {
            return (MenuRole)this.MemberwiseClone();
        }
    }
}
