namespace FOKE.Entity.AppInfoSection.ViewModel
{
    public class AppinfoSectionViewModel : BaseEntityViewModel
    {
        public long Id { get; set; }
        public long? SectionType { get; set; }
        public string? SectionTypeString { get; set; }
        public string HTMLContent { get; set; }
        public long? loggedinUserId { get; set; }
        public bool ShowInWebsite { get; set; }
        public bool ShowInMobile { get; set; }

    }
}
