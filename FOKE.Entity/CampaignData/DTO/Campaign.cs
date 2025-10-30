using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.CampaignData.DTO
{
    public class Campaign : BaseEntity
    {
        [Key]
        public long CampaignId { get; set; }
        public string? CampaignName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? MemberShipFee { get; set; }
        public long? CollectionAdded { get; set; }
        public String? Description { get; set; }

    }
}
