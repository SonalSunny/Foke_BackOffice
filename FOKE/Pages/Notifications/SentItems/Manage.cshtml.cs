using FOKE.Entity.Notification.ViewModel;

//using FOKE.Entity.ProfessionData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Notifications.SentItems
{
    public class ManageModel : PagedListBasePageModel
    {
        public readonly INotificationRepository _notificationRepository;
        public string? pageErrorMessage { get; set; }
        public long? _notificationid { get; set; }
        public IPagedList<NotificationViewModel> pagedListData { get; private set; }
        public ManageModel(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;

        }
        public void OnGet(long? id, string mode, string searchTerm)
        {
            _formMode = mode;
            isValidRequest = true;
            if (id > 0)
            {
                _notificationid = id;
                var retData = _notificationRepository.GetAllNotificationlogs(Convert.ToInt64(id));
                if (retData != null && retData.transactionStatus == System.Net.HttpStatusCode.OK
                             && retData.returnData != null)
                {
                    var result = retData.returnData;

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        searchTerm = searchTerm.ToLower();

                        result = result.Where(x =>
                            (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(searchTerm)) ||
                            (x.ContactNo.HasValue && x.ContactNo.Value.ToString().Contains(searchTerm))
                        ).ToList();
                    }
                    pagedListData = PagedList(result);
                }
            }


        }
        public async Task<IActionResult> OnPost()
        {

            return Page();
        }

    }
}
