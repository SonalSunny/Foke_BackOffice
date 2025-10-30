using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Identity.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string Username { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool? ForceLogin { get; set; }
    }
}
