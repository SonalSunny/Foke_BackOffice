using FOKE.Entity;
using FOKE.Entity.ProjectConfiguration.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IProjectConfigurationRepository
    {
        ResponseEntity<List<ConfigurationViewModel>> GetAllConfigs();
        ResponseEntity<ConfigurationViewModel> GetConfiqbyId(long ConfigId);
        Task<ResponseEntity<ConfigurationViewModel>> EditConfiq(ConfigurationViewModel model);
    }
}
