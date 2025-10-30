using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.ViewModel;

namespace FOKE.Services.Interface
{
    public interface ICommitteeRepository
    {
        Task<ResponseEntity<CommitteViewModel>> AddCommittee(CommitteViewModel model);
        Task<ResponseEntity<CommitteViewModel>> UpdateCommittee(CommitteViewModel model);
        ResponseEntity<CommitteViewModel> GetCommitteeById(long profId);
        ResponseEntity<List<CommitteViewModel>> GetAllCommittee(long? Status);
        ResponseEntity<bool> DeleteCommittee(CommitteViewModel objModel);
    }
}
