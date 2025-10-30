using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ChangePassword.ViewModel
{
    public class ChangePasswordViewModel : BaseEntityViewModel
    {
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long and maximum {1}.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,50}$",
            ErrorMessage = "Password must be 8-50 characters and include letters, numbers, and symbols.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match.")]
        public string? ConfirmPassword { get; set; }

        public long? loggedinUserId { get; set; }



    }
}
