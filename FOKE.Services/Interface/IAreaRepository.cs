using FOKE.Entity;
using FOKE.Entity.AreaMaster.ViewModel;
using FOKE.Entity.Identity.ViewModel;


namespace FOKE.Services.Interface
{
    public interface IAreaRepository
    {
        Task<ResponseEntity<AreaDataViewModel>> AddArea(AreaDataViewModel model);
        Task<ResponseEntity<AreaDataViewModel>> UpdateArea(AreaDataViewModel model);
        ResponseEntity<AreaDataViewModel> GetAreabyId(long profId);
        ResponseEntity<List<AreaDataViewModel>> GetAllAreas(long? Status, string? area);
        ResponseEntity<bool> DeleteArea(AreaDataViewModel objModel);
        Task AssignMembersToAreaAsync(long areaId, List<long> memberIds);

        List<UserViewModel> GetMembersAssignedToArea(long areaId, string? keyword = null, string? column = null);
        Task<List<long>> GetAssignedMemberIdsAsync(long areaId);

        Task<bool> UpdateAssignedUsers(long areaId, List<long>? assignedUserIds, long updatedBy);

    }
}
