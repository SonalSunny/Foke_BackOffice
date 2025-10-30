//using SkakERP.Entities.EmployeeManagement.DTO;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Localization.DTO
{
    public class LocalizationResource : BaseEntity
    {
        [Key]
        public long LocalizationResourceId { get; set; }
        public string Culture { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string CustomValue { get; set; }
    }
}
