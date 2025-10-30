using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.ContactUs.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IcontactUsRepository
    {
        Task<ResponseEntity<bool>> AddClientEnquiery(ContactUsDto model);
        Task<ResponseEntity<List<ClientEnquieryViewModel>>> GetAllClientEnquiery(long? StatusId);
        Task<ResponseEntity<bool>> DeleteEnquiery(ClientEnquieryViewModel objModel);
        Task<ResponseEntity<string>> ExportClientRequestDatatoExcel(string search, long? statusid);
        Task<ResponseEntity<bool>> ResolveIssue(long? Id, string Comment);
    }
}
