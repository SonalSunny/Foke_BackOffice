using FOKE.Entity;
using FOKE.Entity.Localization.ViewModel;

namespace FOKE.Localization.Services
{
    public interface ILocalizationService
    {
        Task<ResponseEntity<bool>> SyncLanguageResources();
        LanguageResourceModel GetStringResource(string resourceKey, string culture);
        ResponseEntity<List<LocalizationResourceModel>> GetLanguageResources(string module);
        ResponseEntity<bool> UpdateLocalizationResource(List<LocalizationResourceModel> localizationResourceModels);
        ResponseEntity<string> ExportLocalizationDatatoExcel(string search);
    }
}