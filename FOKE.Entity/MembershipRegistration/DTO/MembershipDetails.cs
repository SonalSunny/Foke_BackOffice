using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.MembershipRegistration.DTO
{
    public class MembershipDetails : BaseEntity
    {
        [Key]
        public long MembershipId { get; set; }
        public string? Name { get; set; }
        public string? CivilId { get; set; }
        public string? PassportNo { get; set; }
        public DateTime? DateofBirth { get; set; }
        public long? GenderId { get; set; }
        public long? BloodGroupId { get; set; }
        public long? ProffessionId { get; set; }
        public string? ProffessionOther { get; set; }
        public long? CountryCode { get; set; }
        public long? ContactNo { get; set; }
        public long? WhatsAppNoCountryCodeid { get; set; }
        public long? WhatsAppNo { get; set; }
        public string? Email { get; set; }
        public long? AreaId { get; set; }
        public string? Company { get; set; }
        public string? KuwaitAddres { get; set; }
        public long? MembershipType { get; set; }
        public string? PermenantAddress { get; set; }
        public string? Pincode { get; set; }
        public string? EmergencyContactName { get; set; }
        public long? EmergencyContactRelation { get; set; }
        public long? EmergencyContactCountryCodeid { get; set; }
        public long? EmergencyContactNumber { get; set; }
        public string? EmergencyContactEmail { get; set; }
        public long? ParentId { get; set; }

        public long? WorkPlaceId { get; set; }
        public long? DepartmentId { get; set; }
        public string? WorkplaceOther { get; set; }
        public long? HearAboutus { get; set; }
        public long? WorkYear { get; set; }
        public long? DistrictId { get; set; }
        

    }
}
