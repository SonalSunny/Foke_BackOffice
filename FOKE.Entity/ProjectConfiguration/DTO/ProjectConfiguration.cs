using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ProjectConfiguration.DTO
{
    public class ProjectConfiguration : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string? CONFIGKEY { get; set; }
        public string? CONFIGCODE { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
        public string? DefaultValue { get; set; }
    }
}
