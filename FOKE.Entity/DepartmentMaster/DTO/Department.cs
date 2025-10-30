using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.DepartmentMaster.DTO
{
    public class Department : BaseEntity
    {
        [Key]
        public long DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }
    }
}
