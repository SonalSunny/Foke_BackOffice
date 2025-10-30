using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.UnitData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IUnitRepository
    {
        Task<ResponseEntity<UnitViewModel>> AddUnit(UnitViewModel model);
        Task<ResponseEntity<UnitViewModel>> UpdateUnit(UnitViewModel model);
        ResponseEntity<UnitViewModel> GetUnitbyId(long unitId);
        ResponseEntity<List<UnitViewModel>> GetAllUnits(long? Status, string? unit);
        ResponseEntity<bool> DeleteUnit(UnitViewModel objModel);
        Task AssignMembersToUnitAsync(long unitId, List<long> memberIds);

        List<UserViewModel> GetMembersAssignedToUnit(long unitId, string? keyword = null, string? column = null);
        Task<List<long>> GetAssignedMemberIdsAsync(long unitId);

        Task<bool> UpdateAssignedUsers(long unitId, List<long>? assignedUserIds, long updatedBy);

    }
}
