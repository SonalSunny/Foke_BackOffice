using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.WorkPlaceData.DTO
{
    public class WorkPlace : BaseEntity
    {
        [Key]
        public long WorkPlaceId { get; set; }
        public string? WorkPlaceName { get; set; }
        public string? Description { get; set; }
    }
}
