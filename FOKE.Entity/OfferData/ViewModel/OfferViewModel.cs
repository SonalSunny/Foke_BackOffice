using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.OfferData.ViewModel
{
    public class OfferViewModel : BaseEntityViewModel
    {
        public long OfferId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? Heading { get; set; }
        public string? Description { get; set; }
        public bool ShowInWebsite { get; set; }
        public bool ShowInMobile { get; set; }
        public string? ImagePath { get; set; }
        public long? FileStorageId { get; set; }
        public string? ImageName { get; set; }
        public bool AttachmentAny { get; set; }
        public IFormFile? Image { get; set; }
        public string? loggedinUserId { get; set; }

    }
}
