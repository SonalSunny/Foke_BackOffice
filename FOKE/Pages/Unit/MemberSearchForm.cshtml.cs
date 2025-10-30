using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace FOKE.Pages.Unit
{
    public class MemberSearchFormModel : PagedListBasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public PostMembershipViewModel inputModel { get; set; }
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IUserRepository _userRepository;
        public MemberSearchFormModel(ISharedLocalizer sharedLocalizer, IMembershipFormRepository membershipFormRepository, IUnitRepository unitRepository, IUserRepository userRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _membershipFormRepository = membershipFormRepository;
            _unitRepository = unitRepository;
            _userRepository = userRepository;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_STATUS"] = null;
            }
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "UserName", ColumnDescription = _sharedLocalizer.Localize("User Name").Value });
            pageListFilterColumns = objList;


        }
        public IActionResult OnGetGetDetails(string keyword, string column)
        {
            var response = _userRepository.GetMemberDetails(keyword, column);

            // Return entire response as JSON
            return new JsonResult(response);
        }

        // AJAX handler method


        public class MemberAssignmentRequest
        {
            public long UnitId { get; set; }
            public List<long> MemberIds { get; set; }
        }

        public async Task<JsonResult> OnGetAssignedMemberIdsAsync(long unitId)
        {
            var assignedMemberIds = await _unitRepository.GetAssignedMemberIdsAsync(unitId);
            return new JsonResult(assignedMemberIds);
        }
        public async Task<IActionResult> OnPostAddMembersToUnit()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var request = JsonSerializer.Deserialize<MemberAssignmentRequest>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (request?.MemberIds == null || request.MemberIds.Count == 0)
            {
                return new JsonResult(new { success = false, message = "No members selected." });
            }

            try
            {
                await _unitRepository.AssignMembersToUnitAsync(request.UnitId, request.MemberIds);
                return new JsonResult(new { success = true, message = "Members assigned successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


    }





}
