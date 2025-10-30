using FOKE.Entity.Identity.DTO;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.UnitData.ViewModel
{
    public class UnitViewModel : BaseEntityViewModel
    {
        public long UnitId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? UnitName { get; set; }
        public string? Description { get; set; }
        public long? loggedinUserId { get; set; }
        public List<Users> AllUsers { get; set; } = new();
        public List<long> AssignedUserIds { get; set; } = new();
    }
}
