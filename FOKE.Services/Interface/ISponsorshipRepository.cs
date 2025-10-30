using FOKE.Entity;
using FOKE.Entity.SponsorshipData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface ISponsorshipRepository
    {
        Task<ResponseEntity<SponsorshipViewModel>> AddSponsor(SponsorshipViewModel model);
        Task<ResponseEntity<SponsorshipViewModel>> UpdateSponsor(SponsorshipViewModel model);
        ResponseEntity<bool> DeleteSponsor(SponsorshipViewModel objModel);
        ResponseEntity<List<SponsorshipViewModel>> GetAllSponsorData(long? Status);
        ResponseEntity<SponsorshipViewModel> GetSponsorByID(long SponsorId);
    }
}
