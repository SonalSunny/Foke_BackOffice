using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.ViewModel;

namespace FOKE.Services.Interface
{
    public interface ICommitteGroupRepository
    {
        Task<ResponseEntity<CommitteGroupViewModel>> AddCommitteeGroup(CommitteGroupViewModel model);
        Task<ResponseEntity<CommitteGroupViewModel>> UpdateCommitteeGroup(CommitteGroupViewModel model);
        ResponseEntity<CommitteGroupViewModel> GetCommitteeGroupById(long profId);
        ResponseEntity<List<CommitteGroupViewModel>> GetAllCommitteeGroups(long? Status);
        ResponseEntity<bool> DeleteGroup(CommitteGroupViewModel objModel);

    }
}
