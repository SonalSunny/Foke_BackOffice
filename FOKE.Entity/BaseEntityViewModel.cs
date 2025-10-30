namespace FOKE.Entity
{
    public class BaseEntityViewModel
    {
        public bool Active { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedDateFormatted => CreatedDate?.ToString("d-MMM-yyyy");
        public int DiffId { get; set; }
        public string? CreatedUsername { get; set; }
    }
}
