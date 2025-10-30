using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.AreaMaster.DTO
{
    public class AreaData : BaseEntity
    {
        [Key]
        public long AreaId { get; set; }
        public string? AreaName { get; set; }
        public string? Description { get; set; }
    }
}
