using FOKE.Entity.Identity.DTO;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ZoneMaster.ViewModel
{
    public class ZoneViewModel : BaseEntityViewModel
    {
        public long? ZoneId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? ZoneName { get; set; }
        public string? Description { get; set; }
        public long? loggedinUserId { get; set; }
        public List<Users> AllUsers { get; set; } = new();
        public List<long> AssignedUserIds { get; set; } = new();
    }
}
