using FOKE.Entity.AreaMaster.DTO;
using FOKE.Entity.Identity.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.AreaMember.DTO
{
    public class AreaMember : BaseEntity
    {
        [Key]
        public long AreaMemberId { get; set; }
        public long AreaId { get; set; }
        [ForeignKey("AreaId")]
        public virtual AreaData Area { get; set; }
        public long UserMemberId { get; set; }
        [ForeignKey("UserMemberId")]
        public virtual Users UserId { get; set; }
    }
}







