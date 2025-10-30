namespace FOKE.Entity.API.DeviceData.ViewModel
{
    public class DeviceDetailHistoryList
    {
        public string? DeviceName { get; set; }
        public string? OrgFileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? LastOpenDateTime { get; set; }
        public DateTime? LastClosedDateTime { get; set; }
        public string? LastOpenDateTimeString { get; set; }
        public string? LastClosedDateTimeString { get; set; }
        public string? CreatedDateTimeString { get; set; }
        public DateTime? LogOutDateTime { get; set; }
        public string? LogOutDateTimeString { get; set; }

    }
}
