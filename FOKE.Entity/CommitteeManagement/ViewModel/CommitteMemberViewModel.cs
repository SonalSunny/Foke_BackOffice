using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.CommitteeManagement.ViewModel
{
    public class CommitteMemberViewModel : BaseEntityViewModel
    {
        public long CommitteMemberId { get; set; }


        public long? IssueId { get; set; }

        public string? MemberName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? GroupId { get; set; }

        public string? GroupName { get; set; }
        public string? CommitteeName { get; set; }

        public string? ImagePath { get; set; }
        public long? FileStorageId { get; set; }

        public string? ImageName { get; set; }

        public IFormFile? Photo { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public long? CountryCodeId { get; set; }
        public long? ContactNo { get; set; }
        public string? PhoneNo { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        [Range(1, int.MaxValue, ErrorMessage = "Sort order must be a positive number.")]
        public int? SortOrder { get; set; }

        public bool AttachmentAny { get; set; }
        public long? loggedinUserId { get; set; }
        public string? AnyValue { get; set; }
    }
}
