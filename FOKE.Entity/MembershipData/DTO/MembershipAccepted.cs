using FOKE.Entity.AreaMaster.DTO;
using FOKE.Entity.CampaignData.DTO;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.ProfessionData.DTO;
using FOKE.Entity.UnitData.DTO;
using FOKE.Entity.WorkPlaceData.DTO;
using FOKE.Entity.ZoneMaster.DTO;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.MembershipData.DTO
{

    [Index(nameof(CivilId), IsUnique = true)]
    public class MembershipAccepted : BaseEntity
    {
        [Key]
        public long IssueId { get; set; }
        public string? ReferanceNo { get; set; }
        public string? Name { get; set; }

        public string CivilId { get; set; }

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
        public long? WhatsAppNoCountryCodeid { get; set; }
        public long? WhatsAppNo { get; set; }
        public string? Email { get; set; }

        public long? AreaId { get; set; }
        [ForeignKey("AreaId")]
        public virtual AreaData AreaData { get; set; }
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


        public long? CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? CampaignAmount { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? AmountRecieved { get; set; }
        public long? PaymentTypeId { get; set; }
        public string? PaymentRemarks { get; set; }
        public DateTime? MembershipRequestedDate { get; set; }
        public long? ApprovedBy { get; set; }
        [ForeignKey("ApprovedBy")]
        public virtual Users Users { get; set; }
        public long? ReferredBy { get; set; }
        public long? WorkYear { get; set; }
        public string? ProffessionOther { get; set; }
        public string? WorkplaceOther { get; set; }
        public long? DepartmentId { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? PaymentReceivedBy { get; set; }
        public bool? EmailOtp { get; set; }
        public bool? MobileOtp { get; set; }
        public string? Updates { get; set; }
       
    

        public long? ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }

        public long? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        public long? HearAboutUsId { get; set; }
        public DateTime? Memberfrom { get; set; }
        public long? DistrictId { get; set; }
        public string? MembershipNo { get; set; }

    }
}
