using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.NewsAndEventsData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface INewsAndEventsRepository
    {

        Task<ResponseEntity<NewsAndEventsViewModel>> CreateNewsAndEvents(NewsAndEventsViewModel model);
        Task<ResponseEntity<NewsAndEventsViewModel>> UpdateNewsAndEvents(NewsAndEventsViewModel model);
        ResponseEntity<bool> DeleteNewsAndEvents(NewsAndEventsViewModel model);
        ResponseEntity<List<NewsAndEventsViewModel>> GetAllNewsAndEvents(long? Status);
        ResponseEntity<NewsAndEventsViewModel> GetNewsAndEventsByID(long newsId);
        Task<ResponseEntity<bool>> DeleteAttachment(long attachmentId);
        Task<ResponseEntity<NewsAndEventsPageData>> GetAllNewsEventsData(long Type, int Pagenumber, string civilId, long devicePrimaryID);
        Task<ResponseEntity<List<EventTypes>>> GetAllEventTypes();
    }
}
