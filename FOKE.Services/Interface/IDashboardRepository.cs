using FOKE.Entity;
using FOKE.Entity.DashBoard;

namespace FOKE.Services.Interface
{
    public interface IDashboardRepository
    {
        ResponseEntity<DashBoardData> GetMemberDataWithpaidandunpaid();
        ResponseEntity<List<MemberData>> GetMembersByAgeGroup(long type);
        ResponseEntity<List<MemberData>> GetMembersByExperience(long type);

    }
}
