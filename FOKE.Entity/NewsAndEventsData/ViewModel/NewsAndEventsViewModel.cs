using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.NewsAndEventsData.ViewModel
{
    public class NewsAndEventsViewModel : BaseEntityViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? Heading { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }
        public bool ShowInWebsite { get; set; }
        public bool ShowInMobile { get; set; }
        [Required(ErrorMessage = "Required")]
        public long? Type { get; set; }
        public string? DateFormatted => Date?.ToString("d-MMM-yyyy");
        public long? loggedinUserId { get; set; }
        public string? ImagePath { get; set; }
        public long? FileStorageId { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public bool AttachmentAny { get; set; }
        //public List<IFormFile> Attachment { get; set; }
    }
}
