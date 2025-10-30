using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.MembershipRegistration.ViewModel
{
    public class MembershipViewModel : BaseEntityViewModel
    {
        public long MembershipId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string CivilId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? PassportNo { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? DOB { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? Genderid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? BloodGroupid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? Professionid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? WorkPlaceid { get; set; }
        public long? CountryCodeid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? ContactNo { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long Districtid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long Areaid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string Email { get; set; }
        public string? Gender { get; set; }
        public string? BloodGroup { get; set; }
        public string? Proffession { get; set; }
        public string? WorkPlace { get; set; }
        public string? PhoneNo { get; set; }
        public string? District { get; set; }

        public string? Area { get; set; }
        public long? Hearaboutusid { get; set; }

        public string? HearAboutus { get; set; }
        public string? ProffessionOther { get; set; }
        public string? WorkplaceOther { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? WorkYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Professionid == 0 && string.IsNullOrWhiteSpace(ProffessionOther))
            {
                yield return new ValidationResult("REQUIRED", new[] { nameof(Professionid), nameof(ProffessionOther) });
            }
            if (WorkPlaceid == 0 && string.IsNullOrWhiteSpace(WorkplaceOther))
            {
                yield return new ValidationResult("REQUIRED", new[] { nameof(WorkPlaceid), nameof(WorkplaceOther) });
            }
        }
        [Required(ErrorMessage = "REQUIRED")]
        public long? DepartmentId { get; set; }
    }
}
