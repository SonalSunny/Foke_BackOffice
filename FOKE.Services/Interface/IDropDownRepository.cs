using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.Common;

namespace FOKE.Services.Interface
{
    public interface IDropDownRepository
    {
        List<DropDownViewModel> GetRole();
        List<DropDownViewModel> GetLookupTypeList();
        List<DropDownViewModel> GetGender();
        List<DropDownViewModel> GetBloodGroup();
        List<DropDownViewModel> GetProffession();
        List<DropDownViewModel> GetWorkPlace();
        List<DropDownViewModel> GetDistrict();
        List<DropDownViewModel> GetArea();
        List<DropDownViewModel> GetHearAboutUs();
        List<DropDownViewModel> GetUnit();
        List<DropDownViewModel> GetZone();
        List<DropDownViewModel> GetPaymentTypes();
        List<CampaignDropDownList> GetCampaignList();
        List<DropDownViewModel> GetFolder();
        List<DropDownViewModel> GetYearList();
        List<DropDownViewModel> GetDepartmentList();
        List<DropDownViewModel> GetRejectedReasonList();
        List<DropDownViewModel> GetAreasByUser(long userId);
        List<DropDownViewModel> GetUnitsByUser(long userId);
        List<DropDownViewModel> GetZonesByUser(long userId);
        List<DropDownViewModel> GetCancelledReasonList();
        List<DropDownViewModel> GetTypeList();
        List<DropDownViewModel> GetCommitteeList();
        List<DropDownViewModel> GetGroupList();
        List<DropDownViewModel> GetSectionTypeList();
        List<DropDownViewModel> GetNotificationTypeList();
        List<DropDownViewModel> GetUsers();
        List<DropDownViewModel> GetAllCampaignList();
        List<DropDownViewModel> GetAccountTypes();
        List<DropDownViewModel> GetCategoryTypes();
        List<DropDownViewModel> GetallCampaignYears();
        List<DropDownViewModel> GetRelationTypes();
    }
}




