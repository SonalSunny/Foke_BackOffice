using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.ZoneMaster.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IZoneRepository
    {

        Task<ResponseEntity<ZoneViewModel>> AddZone(ZoneViewModel model);
        Task<ResponseEntity<ZoneViewModel>> UpdateZone(ZoneViewModel model);
        ResponseEntity<ZoneViewModel> GetZonebyId(long zoneId);
        ResponseEntity<List<ZoneViewModel>> GetAllZones(long? Status, string? zone);
        ResponseEntity<bool> DeleteZone(ZoneViewModel objModel);
        Task AssignMembersToZoneAsync(long zoneId, List<long> memberIds);

        List<UserViewModel> GetMembersAssignedToZone(long zoneId, string? keyword = null, string? column = null);
        Task<List<long>> GetAssignedMemberIdsAsync(long zoneId);
        Task<bool> UpdateAssignedUsers(long zoneId, List<long>? assignedUserIds, long updatedBy);
    }
}
