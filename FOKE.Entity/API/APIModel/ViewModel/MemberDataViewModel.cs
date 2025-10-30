namespace FOKE.Entity.API.DeviceLogin.ViewModel
{
    public class MemberDataViewModel : BaseEntityViewModel
    {
        public string? Name { get; set; }
        public string? MembershipId { get; set; }
        public long? PhoneNumber { get; set; }
        public bool OTPRequired { get; set; }
        public bool IsBlackListed { get; set; }
        public bool IsBlocked { get; set; }
        public string? Token { get; set; }
        public bool IsRegistered { get; set; }
        public string? CivilId { get; set; }

    }
}
