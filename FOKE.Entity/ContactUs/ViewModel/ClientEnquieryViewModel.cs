namespace FOKE.Entity.ContactUs.ViewModel
{
    public class ClientEnquieryViewModel : BaseEntityViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ContactNo { get; set; }
        public string? DateOfCreation { get; set; }
        public string? Type { get; set; }
        public string? ResolvedBy { get; set; }
        public string? ResolvedDate { get; set; }
        public bool IsResolved { get; set; }
    }
}
