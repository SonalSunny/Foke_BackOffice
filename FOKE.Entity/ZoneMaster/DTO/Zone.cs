using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ZoneMaster.DTO
{
    public class Zone : BaseEntity
    {
        [Key]
        public long ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public string? Description { get; set; }
    }
}
