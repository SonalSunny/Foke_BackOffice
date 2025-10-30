using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Identity.ViewModel
{
    public class RoleViewModel : BaseEntityViewModel
    {

        public long? RoleId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Role Name must be between 1 and 100 characters")]
        public string? RoleName { get; set; }

        public string? RoleDescription { get; set; }
        public long? loggedinUserId { get; set; }
        public bool Active { get; set; }
    }
}
