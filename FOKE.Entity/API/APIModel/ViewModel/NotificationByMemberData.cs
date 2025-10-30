namespace FOKE.Entity.API.APIModel.ViewModel
{
    public class NotificationByMemberData
    {
        public long? Id { get; set; }
        public string? Header { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public string? CreayedDateStringKuwait { get; set; }

    }
}
