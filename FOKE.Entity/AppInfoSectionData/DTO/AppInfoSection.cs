using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.AppInfoSectionData.DTO
{
    public class AppInfoSection : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long? SectionType { get; set; }
        public string HTMLContent { get; set; }
        public bool ShowInWebsite { get; set; }
        public bool ShowInMobile { get; set; }
    }
}
