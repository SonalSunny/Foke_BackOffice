using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.CampaignData.ViewModel
{
    public class CampaignViewModel : BaseEntityViewModel, IValidatableObject
    {
        public long CampaignId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? CampaignName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public decimal? MemberShipFee { get; set; }
        public string? Description { get; set; }
        public string? StartDateFormatted => StartDate?.ToString("d-MMM-yyyy");
        public string? EndDateFormatted => EndDate?.ToString("d-MMM-yyyy");
        public long CollectionAdded { get; set; }
        public long? loggedinUserId { get; set; }
        // ✅ Add these for JS date comparison (yyyy-MM-dd format)
        public string? StartDateIso => StartDate?.ToString("yyyy-MM-dd");
        public string? EndDateIso => EndDate?.ToString("yyyy-MM-dd");
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
            {
                yield return new ValidationResult(
                    "Start Date cannot be greater than End Date.",
                    new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}
