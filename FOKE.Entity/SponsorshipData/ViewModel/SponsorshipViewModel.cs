using Microsoft.AspNetCore.Http;

namespace FOKE.Entity.SponsorshipData.ViewModel
{
    public class SponsorshipViewModel : BaseEntityViewModel
    {
        public long SponsorshipId { get; set; }
        public string? SponsorshipName { get; set; }
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public long? CampaignId { get; set; }
        public string? SponsorShipLabel { get; set; }
        public long? SortOrder { get; set; }
        public string? Notes { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
        public string? CampaignName { get; set; }
        public bool AttachmentAny { get; set; }

        public long? loggedinUserId { get; set; }
        public long? FileStorageId { get; set; }

    }
}
