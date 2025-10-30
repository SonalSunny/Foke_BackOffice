using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.ProjectConfiguration.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class ProjectConfigurationRepository : IProjectConfigurationRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public ProjectConfigurationRepository(FOKEDBContext skakErpDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = skakErpDBContext;

            try
            {
                claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
                var isAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false;
                if (isAuthenticated)
                {
                    var userIdentity = claimsPrincipal?.Identity?.Name;
                    if (userIdentity != null)
                    {
                        long userid = 0;
                        Int64.TryParse(userIdentity, out userid);
                        if (userid > 0)
                        {
                            loggedInUser = userid;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
        public ResponseEntity<List<ConfigurationViewModel>> GetAllConfigs()
        {
            var retModel = new ResponseEntity<List<ConfigurationViewModel>>();
            try
            {
                var objModel = new List<ConfigurationViewModel>();

                var retData = _dbContext.ProjectConfigurations.Where(c => c.Active == true);
                objModel = retData.Select(c => new ConfigurationViewModel()
                {
                    ConfigId = c.Id,
                    CONFIGKEY = c.CONFIGKEY,
                    CONFIGCODE = c.CONFIGCODE,
                    Value = c.Value,
                    Description = c.Description,
                    DefaultValue = c.DefaultValue
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }
            return retModel;
        }
        public ResponseEntity<ConfigurationViewModel> GetConfiqbyId(long ConfigId)
        {
            var retModel = new ResponseEntity<ConfigurationViewModel>();
            try
            {
                var objTeam = _dbContext.ProjectConfigurations
                     .SingleOrDefault(u => u.Id == ConfigId);
                var objModel = new ConfigurationViewModel();
                objModel.ConfigId = objTeam.Id;
                objModel.CONFIGKEY = objTeam.CONFIGKEY;
                objModel.CONFIGCODE = objTeam.CONFIGCODE;
                objModel.Description = objTeam.Description;
                objModel.Value = objTeam.Value;
                objModel.DefaultValue = objTeam.DefaultValue;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public Task<ResponseEntity<ConfigurationViewModel>> EditConfiq(ConfigurationViewModel objModel)
        {
            var retModel = new ResponseEntity<ConfigurationViewModel>();
            try
            {

                var modelData = _dbContext.ProjectConfigurations.Find(objModel.ConfigId);
                if (modelData != null)
                {
                    modelData.Value = objModel.Value;
                    modelData.Description = objModel.Description;
                    modelData.UpdatedBy = loggedInUser;
                }
                _dbContext.SaveChanges();
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Saved Successfully";
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error Occured";
            }
            return Task.FromResult(retModel);
        }
    }
}
