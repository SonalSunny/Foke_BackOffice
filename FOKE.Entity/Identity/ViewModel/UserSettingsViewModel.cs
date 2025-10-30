using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Identity.ViewModel
{
    public class UserSettingsViewModel
    {
        public long UserId { get; set; }
        public string ProfilePhotoBase64 { get; set; }
        public bool IsProfilePhotoUpdated { get; set; }
        public bool AllowEmailCommunication { get; set; }
        public bool AllowPhoneCommunication { get; set; }
        public string ContactNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
