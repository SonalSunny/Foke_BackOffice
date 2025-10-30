namespace FOKE.Entity.Localization.ViewModel
{
    public class LocalizationResourceModel
    {
        public long LocalizationResourceId { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocalizationEnglish { get; set; }
        public string LocalizationArabic { get; set; }
    }

    public class LanguageResourceModel
    {
        public string Culture { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string CustomValue { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
    }
}
