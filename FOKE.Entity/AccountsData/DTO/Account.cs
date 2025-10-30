using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.AccountsData.DTO
{
    public class Account : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long? Category { get; set; }
        public long? CategoryType { get; set; }
        public DateTime? Date { get; set; }
        public long? TotalAmount { get; set; }
        public string? RefNo { get; set; }
        public string? Remarks { get; set; }

    }
}
