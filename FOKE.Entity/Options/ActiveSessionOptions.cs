namespace FOKE.Entity.Options
{
    public class ActiveSessionOptions
    {
        public long? UserId { get; set; }
        public string ActiveSessionId { get; set; }
        public DateTime? LastActiveDateTime { get; set; }
    }
}
