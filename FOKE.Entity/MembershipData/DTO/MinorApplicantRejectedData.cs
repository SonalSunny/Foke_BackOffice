using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.MembershipData.DTO
{
    public class MinorApplicantRejectedData : BaseEntity
    {
        [Key]
        public long MembershipId { get; set; }
        public long? RelationType { get; set; }
        public string? Name { get; set; }
        public string? CivilId { get; set; }
        public string? PassportNo { get; set; }
        public DateTime? DateofBirth { get; set; }
        public long? GenderId { get; set; }
        public long? BloodGroupId { get; set; }
        public long? ProffessionId { get; set; }
        public long? CountryCode { get; set; }
        public long? ContactNo { get; set; }
        public string? Email { get; set; }
        public long? AreaId { get; set; }
        public string? ProffessionOther { get; set; }
        public string? KuwaitAddres { get; set; }
        public string? PermenantAddress { get; set; }
        public string? Pincode { get; set; }
        public long? ParentId { get; set; }
    }
}
