namespace FOKE.Entity.Identity.ViewModel
{
    public class UserProfileViewModel : BaseEntityViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string ProfilePhotoBase64 { get; set; }
        public string LastLoggedInDatetimeFormatted { get; set; }
        public string LastActiveDatetimeFormatted { get; set; }
        public bool ByPassPasswordExpiryPolicy { get; set; }
        public int PasswordExpiresIn { get; set; }
        public long? CustomerID { get; set; }
    }
}
