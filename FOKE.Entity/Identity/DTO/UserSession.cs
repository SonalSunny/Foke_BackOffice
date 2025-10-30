using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.Identity.DTO
{
    public class UserSession : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long LoggedInUserId { get; set; }
        [ForeignKey("LoggedInUserId")]
        public virtual Users User { get; set; }
        public string SessionId { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }

    }
}
