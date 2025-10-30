namespace FOKE.Localization.Models
{
    public class Language
    {
        public string Name { get; set; }
        public string Culture { get; set; }
        public string Direction { get; set; }
    }
    public class ResourceModule
    {
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ResourceFile { get; set; }

    }
}
