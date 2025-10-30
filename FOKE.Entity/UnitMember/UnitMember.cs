using FOKE.Entity.Identity.DTO;
using FOKE.Entity.UnitData.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.UnitMember
{
    public class UnitMember : BaseEntity
    {
        [Key]
        public long UnitMemberId { get; set; }
        public long UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit unit { get; set; }
        public long UserMemberId { get; set; }
        [ForeignKey("UserMemberId")]
        public virtual Users UserId { get; set; }

    }
}
