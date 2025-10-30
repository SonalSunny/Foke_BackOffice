namespace FOKE.Entity
{
    public class HomeViewModel
    {

        public DateTime? CurrentLogin { get; set; }
        public DateTime? PreviousLogin { get; set; }
        public DateTime? PasswordExpiryDate { get; set; }
        public string CurrentLoginDatetimeFormatted { get; set; }

        public string PreviousLoginDatetimeFormatted { get; set; }




    }
}
