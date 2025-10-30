using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.RoleData.DTO
{
    public class RoleGroupClaim : BaseEntity
    {
        [Key]
        public long RoleGroupClaimId { get; set; }
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        public string PageCode { get; set; }
        public string ClaimType { get; set; }
        public bool ClaimValue { get; set; }
    }
}
