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
        public long? WorkPlaceId { get; set; }
        public long? CountryCode { get; set; }
        public long? ContactNo { get; set; }
        public string? Email { get; set; }
        public long? DistrictId { get; set; }
        public long? AreaId { get; set; }
        public long? HearAboutus { get; set; }
        public long? WorkYear { get; set; }
        public string? ProffessionOther { get; set; }
        public string? WorkplaceOther { get; set; }
        public long? DepartmentId { get; set; }
    }
}
