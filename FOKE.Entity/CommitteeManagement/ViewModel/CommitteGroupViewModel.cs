using System.ComponentModel.DataAnnotations;



namespace FOKE.Entity.CommitteeManagement.ViewModel
{
    public class CommitteGroupViewModel : BaseEntityViewModel
    {
        public long GroupId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? GroupName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public int? SortOrder { get; set; }
        [Required(ErrorMessage = "REQUIRED")]

        public long? CommitteeId { get; set; }
        public string? CommitteeName { get; set; }
        public long? loggedinUserId { get; set; }

    }
}







