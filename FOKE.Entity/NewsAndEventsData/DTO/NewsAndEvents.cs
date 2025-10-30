using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.NewsAndEventsData.DTO
{
    public class NewsAndEvent : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string? Heading { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public bool ShowInWebsite { get; set; }
        public bool ShowInMobile { get; set; }
        public long? Type { get; set; }
        public string? ImagePath { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
    }
}
