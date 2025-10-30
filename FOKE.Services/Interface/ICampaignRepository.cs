using FOKE.Entity;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.MembershipFee.ViewModel;

namespace FOKE.Services.Interface
{
    public interface ICampaignRepository
    {
        Task<ResponseEntity<CampaignViewModel>> AddCampaign(CampaignViewModel model);
        Task<ResponseEntity<CampaignViewModel>> UpdateCampaign(CampaignViewModel model);
        ResponseEntity<CampaignViewModel> GetCampaignId(long profId);
        ResponseEntity<List<CampaignViewModel>> GetAllCampaign(long? Status);
        Task<bool> CreateCollectionSheetAsync(long campaignId);
        Task<ResponseEntity<bool>> UpdateMembershipFeeAsync(MembershipFeeViewModel model);
        Task<bool> ToggleCampaignStatusAsync(long campaignId, bool activate);
         Task<int> GetActiveCampaignCountAsync();

    }
}
