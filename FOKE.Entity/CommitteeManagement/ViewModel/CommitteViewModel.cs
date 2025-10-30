using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.CommitteeManagement.ViewModel
{
    public class CommitteViewModel : BaseEntityViewModel
    {
        public long CommitteeId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? CommitteeName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? ToDate { get; set; }
        [Required(ErrorMessage = "REQUIRED")]

        public string? FromDateFormatted => FromDate?.ToString("d-MMM-yyyy");
        public string? ToDateFormatted => ToDate?.ToString("d-MMM-yyyy");
        public int SortOrder { get; set; }
        public long? loggedinUserId { get; set; }
    }
}
