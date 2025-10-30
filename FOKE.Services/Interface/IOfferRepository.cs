using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.OfferData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IOfferRepository
    {
        Task<ResponseEntity<OfferViewModel>> AddOffer(OfferViewModel model);
        Task<ResponseEntity<OfferViewModel>> UpdateOffer(OfferViewModel model);
        ResponseEntity<bool> DeleteOffer(OfferViewModel objModel);
        ResponseEntity<List<OfferViewModel>> GetAllOfferData(long? Status);
        ResponseEntity<OfferViewModel> GetOfferByID(long OfferID);
        ResponseEntity<string> ExportUserDatatoExcel(string search, long? statusid);
        Task<ResponseEntity<List<OfferData>>> GetAllOffers();

    }
}
