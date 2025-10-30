using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.CommitteeManagement.DTO
{
    public class Committee : BaseEntity
    {
        [Key]
        public long CommitteeId { get; set; }
        public string CommitteeName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? SortOrder { get; set; }
    }
}


