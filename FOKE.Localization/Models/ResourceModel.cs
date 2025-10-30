namespace FOKE.Localization.Models
{
    public class ResourceModel
    {
        public string Module { get; set; }
        public List<Translation> Translations { get; set; }
    }
    public class Translation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Localization> Localizations { get; set; }
    }
    public class Localization
    {
        public string Culture { get; set; }
        public string Value { get; set; }
    }
    public class ResourceBaseModel
    {
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }

        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string customValue { get; set; }
    }
}
