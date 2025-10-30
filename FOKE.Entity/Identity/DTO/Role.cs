using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.RoleData.DTO
{
    public class Role : BaseEntity
    {
        [Key]
        public long? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public virtual List<RoleGroupClaim> RoleGroupClaims { get; set; }
    }
}
