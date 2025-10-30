using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.AppInfoSection.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IappInfoSectionRepository
    {
        Task<ResponseEntity<AppinfoSectionViewModel>> AddAppInfoSection(AppinfoSectionViewModel model);
        Task<ResponseEntity<AppinfoSectionViewModel>> UpdateAppInfoSection(AppinfoSectionViewModel model);
        ResponseEntity<bool> DeleteAppInfo(AppinfoSectionViewModel objModel);
        ResponseEntity<List<AppinfoSectionViewModel>> GetAllAppInfoData(long? Status);
        ResponseEntity<AppinfoSectionViewModel> GetAppInfoDataByID(long Id);
        Task<ResponseEntity<List<SectionTypes>>> GetAllSectionTypes();
        Task<ResponseEntity<AppInfoSectionDto>> GetSectionDataByType(long? Type);
        Task<ResponseEntity<bool>> UpdateLoginLogOutTime(long Type, long DeviceId);
    }
}
