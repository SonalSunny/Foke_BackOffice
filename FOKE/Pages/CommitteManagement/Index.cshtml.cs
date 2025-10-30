using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Entity.Common;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.CommitteManagement
{
    public class IndexModel : PagedListBasePageModel
    {
        public IPagedList<CommitteViewModel> pagedListData { get; set; }
        public IPagedList<CommitteGroupViewModel> pagedListDataGroups { get; set; }
        public IPagedList<CommitteMemberViewModel> pagedListDataMembers { get; set; }

        [BindProperty]
        public long? CommitteSearch { get; set; }
        [BindProperty]
        public long? GroupSearch { get; set; }
        [BindProperty]
        public List<DropDownViewModel> CommitteList { get; set; }
        public List<DropDownViewModel> GroupList { get; set; }
        private readonly ISharedLocalizer _sharedLocalizer;
        public readonly ICommitteeRepository _committeeRepository;
        public readonly ICommitteGroupRepository _committeGroupRepository;
        public readonly ICommitteeMemberRepository _committeeMemberRepository;
        private readonly IDropDownRepository _dropDownRepository;

        public IndexModel(ICommitteeRepository committeeRepository, ISharedLocalizer sharedLocalizer, ICommitteGroupRepository committeGroupRepository, ICommitteeMemberRepository committeeMemberRepository, IDropDownRepository dropDownRepository)
        {
            _committeeRepository = committeeRepository;
            _committeGroupRepository = committeGroupRepository;
            _sharedLocalizer = sharedLocalizer;
            _committeeMemberRepository = committeeMemberRepository;
            _dropDownRepository = dropDownRepository;
        }

        public void OnGet(string isGoBack)
        {
            BindDropdowns();
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {

                TempData["PRO_FILTER_Committe"] = null;
                TempData["PRO_FILTER_Group"] = null;

            }

        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();

            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Position", ColumnDescription = _sharedLocalizer.Localize("Position").Value });
            objList.Add(new PageListFilterColumns { ColumName = "PhoneNo", ColumnDescription = _sharedLocalizer.Localize("Contact No").Value });
            objList.Add(new PageListFilterColumns { ColumName = "GroupName", ColumnDescription = _sharedLocalizer.Localize("Group").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CommitteeName", ColumnDescription = _sharedLocalizer.Localize("Committe").Value });
            pageListFilterColumns = objList;
        }

        public IActionResult OnGetPagedListCommittees(int? pn, int? ps, string so, string sc)
        {


            pageNo = pn ?? 1;
            pageSize = ps ?? 5;
            sortOrder = so;
            sortColumn = sc;

            var objList = _committeeRepository.GetAllCommittee(1);
            if (objList != null && objList.transactionStatus == System.Net.HttpStatusCode.OK
         && objList.returnData != null)
            {
                pagedListData = PagedList(objList.returnData);
            }
            return new PartialViewResult
            {
                ViewName = "_CommitteeIndexPartial",
                ViewData = ViewData
            };
        }

        public IActionResult OnGetPagedListGroups(int? pn, int? ps, string so, string sc)
        {

            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 5;  // Default page size
            sortOrder = so;
            sortColumn = sc;

            var objList = _committeGroupRepository.GetAllCommitteeGroups(1);
            if (objList != null && objList.transactionStatus == System.Net.HttpStatusCode.OK
         && objList.returnData != null)
            {
                pagedListDataGroups = PagedList(objList.returnData);//objList.returnData.ToPagedList((int)pageNo, (int)pageSize);
            }
            return new PartialViewResult
            {
                ViewName = "_GroupIndexPartial",
                ViewData = ViewData
            };
        }
        public IActionResult OnGetPagedListMembers(int? pn, int? ps, string so, string sc, string gs, string gsc, long? status)
        {

            setPagedListColumns();
            pageNo = pn ?? 1;
            pageSize = ps ?? 10;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;

            var Committee = TempData.Peek("PRO_FILTER_Committe");
            CommitteSearch = GenericUtilities.Convert<long?>(Committee);
            var Grp = TempData.Peek("PRO_FILTER_Group");
            GroupSearch = GenericUtilities.Convert<long?>(Grp);
            var result = _committeeMemberRepository.GetAllCommitteeMembers(1, CommitteSearch, GroupSearch);

            if (result != null && result.transactionStatus == System.Net.HttpStatusCode.OK && result.returnData != null)
            {
                pagedListDataMembers = PagedList(result.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_MemberIndexPartial",
                ViewData = ViewData
            };
        }
        private void BindDropdowns()
        {
            CommitteList = _dropDownRepository.GetCommitteeList();
            GroupList = _dropDownRepository.GetGroupList();
        }

        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_Committe"] = CommitteSearch.ToString();
            TempData["PRO_FILTER_Group"] = GroupSearch.ToString();

            return new JsonResult(true);

        }

        public JsonResult OnPostDeleteCommittee(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new CommitteViewModel();
            objModel.CommitteeId = Convert.ToInt32(keyid);
            retData = _committeeRepository.DeleteCommittee(objModel);
            return new JsonResult(retData);
        }

        public JsonResult OnPostDeleteGroup(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new CommitteGroupViewModel();

            objModel.GroupId = Convert.ToInt32(keyid);
            retData = _committeGroupRepository.DeleteGroup(objModel);
            return new JsonResult(retData);
        }
        public JsonResult OnPostDeleteMember(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new CommitteMemberViewModel();
            objModel.CommitteMemberId = Convert.ToInt32(keyid);
            retData = _committeeMemberRepository.DeleteMember(objModel);
            return new JsonResult(retData);
        }

    }
}
