using FOKE.Entity;
using FOKE.Entity.MembershipIssuedData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IReportRepository
    {
        ResponseEntity<List<PostMembershipViewModel>> GetPaymentSummaryReport(long? UserId, string? Year = null);
        ResponseEntity<List<PostMembershipViewModel>> GetPaymentDetailReport(long? Area, long? Unit, long? Zone, long? UserId, DateTime? FromDate, DateTime? ToDate, long? CampaignID, int? pn, int? ps, string? so, string? sc);
    }
}
