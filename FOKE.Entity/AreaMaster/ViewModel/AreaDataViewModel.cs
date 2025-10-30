using FOKE.Entity.Identity.DTO;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.AreaMaster.ViewModel
{
    public class AreaDataViewModel : BaseEntityViewModel
    {
        public long? AreaId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? AreaName { get; set; }
        public string? Description { get; set; }

        public long? loggedinUserId { get; set; }
        // For checkboxes
        public List<Users> AllUsers { get; set; } = new();
        public List<long> AssignedUserIds { get; set; } = new();
    }
}
