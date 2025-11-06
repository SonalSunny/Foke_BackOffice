using FOKE.Entity.API.DeviceData.ViewModel;
using FOKE.Entity.MembershipRegistration.ViewModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.MembershipIssuedData.ViewModel
{
    public class PostMembershipViewModel : BaseEntityViewModel
    {
        public long? IssueId { get; set; }
        public long MembershipId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Civil ID must be 12 digits.")]


        [RegularExpression(@"^\d{12}$", ErrorMessage = "Only 12 digits are allowed.")]
        public string CivilId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? PassportNo { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? DateofBirth { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? GenderId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? BloodGroupId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? ProfessionId { get; set; }
        public long? CountryCodeId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? ContactNo { get; set; }
        public long? WhatsAppNoCountryCodeid { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? WhatsAppNo { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long AreaId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? CampaignId { get; set; }
        public decimal? AmountRecieved { get; set; }
        public long? PaymentTypeId { get; set; }
        public decimal? CampaignAmount { get; set; }
        public string? PaymentRemarks { get; set; }
        public long? loggedinUserId { get; set; }
        public long MembershipStatus { get; set; }
        public string? Gender { get; set; }
        public string? BloodGroup { get; set; }
        public string? Profession { get; set; }
        public string? PhoneNo { get; set; }
        public string? WhatsappNoString { get; set; }
        public string? Area { get; set; }
        public string? Company { get; set; }
        public string? KuwaitAddress { get; set; }
        public string? CampaignName { get; set; }
        public string? CampaignEndDateString { get; set; }
        public string? PaymentType { get; set; }
        public string? RejectionReason { get; set; }
        public string? RejectionRemarks { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? DateofBirthString { get; set; }
        public string? PaymentRecievedDate { get; set; }
        public string? PaymentRecievedBy { get; set; }
        public DateTime? Memberfrom { get; set; }
        public string? MemberfromString { get; set; }
        public DateTime? MembershipRequestedDate { get; set; }
        public string? MembershipRequestedDateString { get; set; }
        public long? ApprovedBy { get; set; }
        public string? SearchText { get; set; }
        public string? ApprovedByName { get; set; }
        public List<IFormFile>? Attachment { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ProffessionOther { get; set; }
        public string? DepartmentName { get; set; }
        public string? RejectedByName { get; set; }
        public string? RejectedDate { get; set; }
        public long? RejectionReasonId { get; set; }
        public bool PaymentDone { get; set; }
        public string? ReferredByName { get; set; }
        public string? CamAmount { get; set; }
        public List<DeviceDetailViewModel>? DeviceData { get; set; }
        public long? DeviceCount { get; set; }
        public int AmountInt { get; set; }
        public string? Updates { get; set; }
        public string? SelectedRadio { get; set; }
        public string? UserName { get; set; }
        public decimal TotalReceived { get; set; }
        public long? PaymentRecievedByid { get; set; }
        public string? CollectedDateformat { get; set; }
        public DateTime? CollectedDate { get; set; }
        public int? NoOfMembers { get; set; }
        public int? Age { get; set; }
        public long? MembershipFeeId { get; set; }
        public string? LastMembershipAdded { get; set; }

        public List<FamilyMembersData>? FamilyData { get; set; }

        #region Kerela_Details 

        [Required(ErrorMessage = "REQUIRED")]
        public string? PermenantAddress { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? Pincode { get; set; }

        #endregion

        #region EmergencyContact

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string? EmergencyContactName { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? EmergencyContactRelation { get; set; }
        public string? EmergencyContactRelationString { get; set; }
        public long? EmergencyContactCountryCodeid { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
        public long? EmergencyContactNumber { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? EmergencyContactEmail { get; set; }
        public string? EmergencyContactNumberString { get; set; }

        #endregion


        #region OldVariables That are not needed now
        [Required(ErrorMessage = "REQUIRED")]
        public long? DepartmentId { get; set; }
        public string? WorkplaceOther { get; set; }
        public string? HearAboutUs { get; set; }
        public string? WorkPlace { get; set; }
        public long? ReferredBy { get; set; }
        public long? HearAboutUsId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? WorkPlaceId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? ZoneId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? UnitId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProfessionId == 0 && string.IsNullOrWhiteSpace(ProffessionOther))
            {
                yield return new ValidationResult("REQUIRED", new[] { nameof(ProfessionId), nameof(ProffessionOther) });
            }
            if (WorkPlaceId == 0 && string.IsNullOrWhiteSpace(WorkplaceOther))
            {
                yield return new ValidationResult("REQUIRED", new[] { nameof(WorkPlaceId), nameof(WorkplaceOther) });
            }
        }
        public string? Zone { get; set; }
        public string? Unit { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long? WorkYear { get; set; }
        public string? District { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public long DistrictId { get; set; }

        #endregion
    }
}
