using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ProfessionData.DTO
{
    public class Profession : BaseEntity
    {
        [Key]
        public long ProfessionId { get; set; }
        public string? ProffessionName { get; set; }
        public string? Description { get; set; }
    }
}
