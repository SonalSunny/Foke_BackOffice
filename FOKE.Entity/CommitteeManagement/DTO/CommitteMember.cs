using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.MembershipData.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.CommitteeManagement.DTO
{
    public class CommitteMember : BaseEntity
    {
        [Key]
        public long CommitteMemberId { get; set; }
        public long? IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual MembershipAccepted MembershipAccepted { get; set; }
        public long? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Committegroup CommitteGroup { get; set; }
        public string? ImagePath { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public long? CountryCodeId { get; set; }
        public long? ContactNo { get; set; }
        public int? SortOrder { get; set; }
        public string? AnyValue { get; set; }
    }
}
