using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.WorkPlaceData.ViewModel
{
    public class WorkPlaceViewModel : BaseEntityViewModel
    {
        public long WorkPlaceId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? WorkPlaceName { get; set; }
        public string? Description { get; set; }
        public long? loggedinUserId { get; set; }
    }
}
