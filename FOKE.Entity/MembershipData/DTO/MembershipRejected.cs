using FOKE.Entity.AreaMaster.DTO;
using FOKE.Entity.MembershipRegistration.DTO;
using FOKE.Entity.ProfessionData.DTO;
using FOKE.Entity.UnitData.DTO;
using FOKE.Entity.WorkPlaceData.DTO;
using FOKE.Entity.ZoneMaster.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.MembershipData.DTO
{
    public class MembershipRejected : BaseEntity
    {
        [Key]
        public long IssueId { get; set; }

        public long? RegistrationId { get; set; }
        [ForeignKey("RegistrationId")]
        public virtual MembershipDetails MembershipDetails { get; set; }

        public string? ReferanceNo { get; set; }
        public string? Name { get; set; }

        public string? CivilId { get; set; }

        public string? PassportNo { get; set; }

        public DateTime? DateofBirth { get; set; }

        public long? GenderId { get; set; }

        public long? BloodGroupId { get; set; }

        public long? ProfessionId { get; set; }
        [ForeignKey("ProfessionId")]
        public virtual Profession Profession { get; set; }

        public long? WorkPlaceId { get; set; }
        [ForeignKey("WorkPlaceId")]
        public virtual WorkPlace WorkPlace { get; set; }

        public long? CountryCodeId { get; set; }

        public long? ContactNo { get; set; }

        public string? Email { get; set; }

        public long? DistrictId { get; set; }

        public long? AreaId { get; set; }
        [ForeignKey("AreaId")]
        public virtual AreaData AreaData { get; set; }

        public long? ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }

        public long? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        public long? CampaignId { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? CampaignAmount { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? AmountRecieved { get; set; }
        public long? PaymentTypeId { get; set; }
        public string? PaymentRemarks { get; set; }
        public long? HearAboutUsId { get; set; }
        public long? RejectionReasonId { get; set; }
        public string? RejectionReason { get; set; }
        public string? RejectionRemarks { get; set; }
        public long? WorkYear { get; set; }
        public string? ProffessionOther { get; set; }
        public string? WorkplaceOther { get; set; }
        public long? DepartmentId { get; set; }

    }
}
