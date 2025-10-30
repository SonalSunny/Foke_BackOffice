using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ProfessionData.ViewModel
{
    public class ProfessionViewModel : BaseEntityViewModel
    {
        public long ProfessionId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? ProfessionName { get; set; }
        public string? Description { get; set; }
        public long? loggedinUserId { get; set; }
    }
}
