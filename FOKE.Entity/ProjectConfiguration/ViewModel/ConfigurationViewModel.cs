using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ProjectConfiguration.ViewModel
{
    public class ConfigurationViewModel
    {
        public long ConfigId { get; set; }
        public string? CONFIGKEY { get; set; }
        public string? CONFIGCODE { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? Value { get; set; }
        public string? DefaultValue { get; set; }
    }
}
