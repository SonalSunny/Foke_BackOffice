using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Identity.ViewModel
{
    public class UserViewModel : BaseEntityViewModel
    {
        public long UserId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 50 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public long EmployeeID { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 50 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public DateTime? PasswordExpirydate { get; set; }
        public string? PasswordExpirydateFormat { get; set; }
        public string? Culture { get; set; }
        public string? ThemeMode { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long RoleId { get; set; }
        public string? EmployeeName { get; set; }
        public string? RoleName { get; set; }
        public long? loggedinUserId { get; set; }
        public string? CreatedUsername { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? ActiveSessionId { get; set; }
    }
}
