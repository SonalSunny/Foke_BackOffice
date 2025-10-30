using FOKE.Entity.Identity.DTO;
using FOKE.Entity.ZoneMaster.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.ZoneMember
{
    public class ZoneMember : BaseEntity
    {
        [Key]
        public long ZoneMemberId { get; set; }
        public long ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual Zone zone { get; set; }
        public long UserMemberId { get; set; }
        [ForeignKey("UserMemberId")]
        public virtual Users UserId { get; set; }
    }
}
