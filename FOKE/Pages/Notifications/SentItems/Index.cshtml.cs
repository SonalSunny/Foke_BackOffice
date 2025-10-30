using FOKE.Entity;
using FOKE.Entity.Notification.ViewModel;


//using FOKE.Entity.ProfessionData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Notifications.SentItems
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly INotificationRepository _notificationRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        [BindProperty]
        public string? AreaName { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }
        public IPagedList<NotificationViewModel> pagedListData { get; private set; }
        public IndexModel(INotificationRepository notificationRepository, ISharedLocalizer sharedLocalizer)
        {
            _notificationRepository = notificationRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {


            }
        }

        public JsonResult OnPostApplyFilter()
        {

            return new JsonResult(true);
        }

        public IActionResult OnPostExportData()
        {
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            var ProfessionName = TempData.Peek("PRO_FILTER_PROFESSION");
            Statusid = GenericUtilities.Convert<long?>(Status);


            if (Statusid == null)
            {
                Statusid = 1;
            }
            var tempFileName = "";
            return new JsonResult(new { tFileName = tempFileName, fileName = "NotiMaster.xlsx" });


        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "NotificationType", ColumnDescription = _sharedLocalizer.Localize("Type").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Header", ColumnDescription = _sharedLocalizer.Localize("Header").Value });
            objList.Add(new PageListFilterColumns { ColumName = "NotificationContent", ColumnDescription = _sharedLocalizer.Localize("Description").Value });
            pageListFilterColumns = objList;
        }

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);



            var objList = _notificationRepository.GetAllNotifications(Statusid);
            if (objList != null && objList.transactionStatus == System.Net.HttpStatusCode.OK
         && objList.returnData != null)
            {
                pagedListData = PagedList(objList.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
    }
}





