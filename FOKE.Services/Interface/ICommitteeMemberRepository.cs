using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.CommitteeManagement.ViewModel;

namespace FOKE.Services.Interface
{
    public interface ICommitteeMemberRepository
    {
        Task<ResponseEntity<CommitteMemberViewModel>> AddCommitteeMember(CommitteMemberViewModel model);
        Task<ResponseEntity<bool>> UpdateCommitteeMember(CommitteMemberViewModel model);
        ResponseEntity<CommitteMemberViewModel> GetCommitteeMemberById(long memberId);
        ResponseEntity<List<CommitteMemberViewModel>> GetAllCommitteeMembers(long? Status, long? CommitteSearch, long? GroupSearch);
        ResponseEntity<bool> DeleteMember(CommitteMemberViewModel objModel);
        Task<ResponseEntity<List<CommitteByGroupDto>>> GetCommitteDetailsByGroup();
    }
}
