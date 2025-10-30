using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.OfferData.DTO
{
    public class Offer : BaseEntity
    {
        [Key]
        public long OfferId { get; set; }
        public string? Heading { get; set; }
        public string? Description { get; set; }
        public bool ShowInWebsite { get; set; }
        public bool ShowInMobile { get; set; }
        public string? ImagePath { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }

    }
}
