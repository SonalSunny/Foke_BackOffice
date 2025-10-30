using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.CommitteeManagement.DTO
{
    public class Committegroup : BaseEntity
    {
        [Key]
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public long? CommitteeId { get; set; }
        [ForeignKey("CommitteeId")]
        public virtual Committee Committee { get; set; }
        public int? SortOrder { get; set; }
    }
}
