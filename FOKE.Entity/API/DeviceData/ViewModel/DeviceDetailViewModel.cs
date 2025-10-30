namespace FOKE.Entity.API.DeviceData.ViewModel
{
    public class DeviceDetailViewModel : BaseEntityViewModel
    {
        public long? DeviceDetailId { get; set; }
        public string? DeviceId { get; set; }
        public string? CivilId { get; set; }
        public string? Token { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceModel { get; set; }
        public string? DeviceType { get; set; }
        public List<DeviceDetailHistoryList> HistoryList { get; set; }
        public string? OrgFileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? LastOpenDateTime { get; set; }
        public DateTime? LastClosedDateTime { get; set; }
        public string? LastOpenDateTimeString { get; set; }
        public string? LastClosedDateTimeString { get; set; }
        public string? CreatedDateTimeString { get; set; }
        public DateTime? LogOutDateTime { get; set; }
        public string? LogOutDateTimeString { get; set; }
        public bool ForceLogOut { get; set; }
    }
}
