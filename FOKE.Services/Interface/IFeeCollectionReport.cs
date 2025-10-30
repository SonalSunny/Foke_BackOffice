using FOKE.Entity;
using FOKE.Entity.DashBoard;
using FOKE.Entity.MembershipIssuedData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IFeeCollectionReport
    {
        Task<int> GetPaidCountAsync(long campaignId);
        Task<int> GetUnpaidCountAsync(long campaignId);
        public ResponseEntity<List<DashBoardViewModel>> GetAreaCountwithpaidandunpaid(long campaignId);
        public ResponseEntity<List<DashBoardViewModel>> GetZoneCountwithpaidandunpaid(long campaignId);
        public ResponseEntity<List<DashBoardViewModel>> GetUnitCountWithpaidAndunpaid(long campaignId);
        Task<ResponseEntity<List<DashBoardViewModel>>> GetPaymentTypeCountsAsync(long campaignId);
        Task<ResponseEntity<PostMembershipViewModel>> FeeCollection(PostMembershipViewModel model);
        Task<ResponseEntity<List<MemberData>>> GetPaymentCollectorsData(long campaignId);
    }
}
