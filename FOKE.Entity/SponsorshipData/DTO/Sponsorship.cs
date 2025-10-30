using FOKE.Entity.CampaignData.DTO;
using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.SponsorshipData.DTO
{
    public class Sponsorship : BaseEntity
    {
        [Key]
        public long SponsorshipId { get; set; }
        public string? SponsorshipName { get; set; }
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public long? CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }
        public string? SponsorShipLabel { get; set; }
        public long? SortOrder { get; set; }
        public string? Notes { get; set; }

        public string? ImageUrl { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
    }
}
