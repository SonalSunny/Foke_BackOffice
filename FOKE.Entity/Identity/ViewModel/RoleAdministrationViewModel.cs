using FOKE.Entity.MenuManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Identity.ViewModel
{
    public class RoleAdministrationViewModel
    {
        [BindProperty]
        public string RoleCode { get; set; }
        //public string RoleName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "REQUIRED")]
        public long? RoleId { get; set; }
        public string RoleName { get; set; }
        public List<MenuGroup> MenuGroups { get; set; }
    }
}
