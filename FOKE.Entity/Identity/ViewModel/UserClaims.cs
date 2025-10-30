using FOKE.Entity.MenuManagement.DTO;

namespace FOKE.Entity.Identity.ViewModel
{
    public class UserClaims
    {
        public bool IsAdmin { get; set; }
        public List<MenuClaim> claims { get; set; }
        public UserClaims()
        {
            claims = new List<MenuClaim>();
        }
    }
}
