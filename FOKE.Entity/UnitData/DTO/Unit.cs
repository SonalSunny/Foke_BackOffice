using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.UnitData.DTO
{
    public class Unit : BaseEntity
    {
        [Key]
        public long UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? Description { get; set; }

    }
}
