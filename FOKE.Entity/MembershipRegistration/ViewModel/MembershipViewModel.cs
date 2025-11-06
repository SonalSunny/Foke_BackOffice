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
        public string? Gender { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? BloodGroupid { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? Professionid { get; set; }
        public string? Proffession { get; set; }
        public long? CountryCodeid { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? ContactNo { get; set; }
        public long? WhatsAppNoCountryCodeid { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? WhatsAppNo { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long Areaid { get; set; }
        public string? Area { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string Email { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? CompanyName { get; set; }
        public string? BloodGroup { get; set; }
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? KuwaitAddres { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? MembershipType { get; set; }
        //-----------------------------------------//
        public List<FamilyMembersData>? FamilyData { get; set; }
        //-----------------------------------------//
        [Required(ErrorMessage = "REQUIRED")]
        public string? PermenantAddress { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? Pincode { get; set; }
        //-----------------------------------------//

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string? EmergencyContactName { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? EmergencyContactRelation { get; set; }
        public long? EmergencyContactCountryCodeid { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? EmergencyContactNumber { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? EmergencyContactEmail { get; set; }

        //-----------------------------------------//
        public long? WorkPlaceid { get; set; }//maybe not needed
        public string? WorkPlace { get; set; }
        public long? Hearaboutusid { get; set; }
        public string? HearAboutus { get; set; }
        public string? ProffessionOther { get; set; }
        public string? WorkplaceOther { get; set; }
        public long? WorkYear { get; set; }
        public long Districtid { get; set; }//maybe not needed
        public string? District { get; set; }
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
        public long? DepartmentId { get; set; }
    }

    public class FamilyMembersData
    {
        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? RelationType { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string CivilId { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public long? GenderId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? PassportNo { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? BloodGroupid { get; set; }

        public long? CountryCodeid { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? MobileNoRelative { get; set; }
        public string? EmailRelative { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? Professionid { get; set; }
        public string? CompanyName { get; set; }
        public long? ParentId { get; set; }
    }
}
