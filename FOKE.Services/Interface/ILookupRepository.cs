using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;

namespace FOKE.Services.Interface
{
    public interface ILookupRepository
    {
        ResponseEntity<List<LookupViewModel>> GetAllLookups(long? Status, long? LookupTypeId, string? LookUpName);
        ResponseEntity<bool> DeleteTeam(LookupViewModel objModel);
        ResponseEntity<LookupViewModel> GetLookUpbyId(long LookUpId);
        ResponseEntity<string> ExportTeamDatatoExcel(string search, long? statusid, long? LookupTypeId, string? LookUpName);
        Task<ResponseEntity<LookupViewModel>> AddLookUp(LookupViewModel model);
    }
}
