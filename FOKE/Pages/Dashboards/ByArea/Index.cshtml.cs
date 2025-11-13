using FOKE.Entity.DashBoard;
using FOKE.Localization;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FOKE.Pages.Dashboards.ByArea
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardRepository _dashboardRepository;
        public List<DashBoardViewModel> AreaWiseData { get; set; } = new();
        public List<SunburstNode> ZoneWiseData { get; set; }
        public List<SunburstNode> UnitWiseData { get; set; }
        public List<MemberData> BloodGroupedData { get; set; }
        public List<DepartmentGenderData> DepartmentData { get; set; }
        public List<MemberData> GenderData { get; set; }
        public List<MemberData> WorkPlaceData { get; set; }
        public List<MemberData> ProffesionData { get; set; }
        public List<MemberData> AllMemberList { get; set; }
        public List<SunburstNode> AgewiseCategory { get; set; }
        public List<SunburstNode> ExpwiseCategory { get; set; }

        public List<DistrictGenderData> districtGenderDatas { get; set; }
        private readonly ISharedLocalizer _sharedLocalizer;

        public IndexModel(IDashboardRepository dashboardRepository, ISharedLocalizer sharedLocalizer)
        {
            _dashboardRepository = dashboardRepository;
            _sharedLocalizer = sharedLocalizer;


        }
        public void OnGet()
        {
            var retData = _dashboardRepository.GetMemberDataWithpaidandunpaid();
            if (retData.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                AreaWiseData = retData.returnData.AreaData;
                ZoneWiseData = retData.returnData.ZoneData;
                UnitWiseData = retData.returnData.UnitData;
                DepartmentData = retData.returnData.DepartmentData;
                GenderData = retData.returnData.GenderData;
                districtGenderDatas = retData.returnData.DistrictData;
                BloodGroupedData = retData.returnData.BloodGroupData;
                WorkPlaceData = retData.returnData.WorkPlaceData;
                ProffesionData = retData.returnData.ProffesionData;
                AllMemberList = retData.returnData.OrgMemberData;
                //AgewiseCategory = retData.returnData.AgewiseCategory;
                //ExpwiseCategory = retData.returnData.ExpwiseCategory;
            }
        }

        public JsonResult OnGetMembersDataByAgeCategory(long type)
        {
            var ReturnData = _dashboardRepository.GetMembersByAgeGroup(type);
            return new JsonResult(ReturnData.returnData);

        }
        public JsonResult OnGetMembersDataByExperience(long type)
        {
            var ReturnData = _dashboardRepository.GetMembersByExperience(type);
            return new JsonResult(ReturnData.returnData);
        }
    }
}
