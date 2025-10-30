using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Identity.ViewModel
{
    public class LookupViewModel : BaseEntityViewModel
    {
        public long LookUpId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long LookUpTypeId { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string? LookUpName { get; set; }
        public string? LookUpTypeName { get; set; }
        public string? Description { get; set; }
    }
}
