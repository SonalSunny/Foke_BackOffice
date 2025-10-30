using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Notification.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace FOKE.Pages.Notifications.Compose
{
    public class IndexModel : BasePageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMembershipFormRepository _membershipFormRepository;

        #region FilterDeclaration

        public List<ReciepientData> ReciepientData { get; set; }

        [BindProperty]
        public long SortOrder { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedType { get; set; }
        public List<SelectListItem> NotificationType { get; set; }
        [BindProperty]
        public string? Year { get; set; }
        [BindProperty]
        public string? currentType1 { get; set; }
        [BindProperty]
        public string? currentType2 { get; set; }
        [BindProperty]
        public List<DropDownViewModel> NotificationTypeList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> AreaList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> ZoneList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> UnitList { get; set; }
        [BindProperty]
        public NotificationViewModel inputModel { get; set; }

        public List<PageListFilterColumns> pageListFilterColumns { get; set; }
        public string? SearchText { get; set; }

        #endregion
        public IndexModel(ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository, INotificationRepository notificationRepository, IMembershipFormRepository membershipFormRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
            _notificationRepository = notificationRepository;
            _membershipFormRepository = membershipFormRepository;
        }

        public void OnGet(string isGoBack)
        {
            // Initialize notifications dropdown
            InitializeNotificationDropdown();
            BindDropdowns();
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_HOLIDAY_NAME"] = "";
                TempData["PRO_FILTER_STATUS"] = null;
                TempData["PRO_FILTER_HOLIDAY_DATE"] = null;
                TempData["PRO_FILTER_YEAR"] = SelectedType;
            }
        }

        private void InitializeNotificationDropdown()
        {
            currentType1 = "SMS Notifications";
            currentType2 = "Push notifications";
            NotificationType = new List<SelectListItem>
            {
                new SelectListItem($"{currentType1}", currentType1.ToString()) { Selected = true },
                new SelectListItem($"{currentType2}", currentType2.ToString())
            };

        }

        public async Task<IActionResult> OnPost()
        {
            inputModel.NotificationType = SelectedType;
            var retData = new ResponseEntity<NotificationViewModel>();
            if (ModelState.IsValid)
            {
                retData = await _notificationRepository.SaveNotification(inputModel);
                if (retData.transactionStatus != HttpStatusCode.OK)
                {
                    pageErrorMessage = retData.returnMessage;
                    IsSuccessReturn = false;
                }
                else
                {
                    ModelState.Clear();
                    IsSuccessReturn = true;
                    sucessMessage = retData.returnMessage;
                    inputModel = new NotificationViewModel();
                }
            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all the required fields";
                IsSuccessReturn = false;
            }
            InitializeNotificationDropdown();
            BindDropdowns();
            return Page();
        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FullName", ColumnDescription = _sharedLocalizer.Localize("Full Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilID", ColumnDescription = _sharedLocalizer.Localize("Civil ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Passport Number", ColumnDescription = _sharedLocalizer.Localize("Passport Number").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Contact Number", ColumnDescription = _sharedLocalizer.Localize("Contact Number").Value });

            pageListFilterColumns = objList;
        }

        public IActionResult OnGetGetDetails(string keyword, string searchText)
        {
            var response = _notificationRepository.GetMemberDetails(keyword, searchText);
            var reciepientsData = response.returnData.Select(i => new ReciepientData
            {
                IssueId = i.IssueId,
                Name = i.Name,
                PhoneNo = i.PhoneNo,
                Proffession = i.Proffession,
                Workplace = i.Workplace,
                Area = i.Area,
                Zone = i.Zone,
                Unit = i.Unit,
                Department = i.Department,
                RefNo = i.RefNo,
                ImagePath = i.ImagePath,
            }).ToList();
            ReciepientData = reciepientsData;
            // Return entire response as JSON
            return new JsonResult(reciepientsData);
        }

        private void BindDropdowns()
        {
            NotificationTypeList = _dropDownRepository.GetNotificationTypeList();
            AreaList = _dropDownRepository.GetArea();
            ZoneList = _dropDownRepository.GetZone();
            UnitList = _dropDownRepository.GetUnit();

        }

        public IActionResult OnGetMemberCountArea(long? AreaId)
        {
            var ReturnData = _notificationRepository.MembersByArea(AreaId);
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }

        public IActionResult OnGetMemberCountZone(long? Zoneid)
        {
            var ReturnData = _notificationRepository.MembersByZone(Zoneid);
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }

        public IActionResult OnGetMemberCountUnit(long? UnitId)
        {
            var ReturnData = _notificationRepository.MembersByUnit(UnitId);
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }

        public IActionResult OnGetAllMemberCount()
        {
            var ReturnData = _notificationRepository.AllMemberCount();
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }

        public IActionResult OnGetAllCommitteeMemberCount()
        {
            var ReturnData = _notificationRepository.AllCommitteeMemberCount();
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }

        public IActionResult OnGetAllActiveMemberCount()
        {
            var ReturnData = _notificationRepository.AllActiveMemberCount();
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }

        public IActionResult OnGetAllinActiveMemberCount()
        {
            var ReturnData = _notificationRepository.AllInActiveMemberCount();
            ReciepientData = ReturnData.ReciepientData;
            long status = ReturnData.Count;
            return new JsonResult(new { status, reciepientData = ReciepientData });
        }
    }
}
