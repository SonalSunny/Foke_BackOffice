using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ContactUSFromWebsite.DTO
{
    public class ContactForm : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(150, ErrorMessage = "Subject cannot be longer than 150 characters")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Subscribe to Newsletter?")]
        public bool SubscribeToNewsletter { get; set; }

    }
}
