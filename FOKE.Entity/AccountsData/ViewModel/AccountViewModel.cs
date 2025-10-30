using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.AccountsData.ViewModel
{
    public class AccountViewModel : BaseEntityViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? Category { get; set; }
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? Date { get; set; }
        public string? DateFormat { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? CategoryType { get; set; }
        public long? TotalAmount { get; set; }
        public string? RefNo { get; set; }
        public string? Remarks { get; set; }
        public long? loggedinUserId { get; set; }

    }
}
