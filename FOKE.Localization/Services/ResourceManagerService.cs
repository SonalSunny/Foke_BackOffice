
using FOKE.Localization.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace FOKE.Localization.Services
{
    public class ResourceManagerService : IResourceManagerService
    {

        public ResourceManagerService(/*IWebHostEnvironment hostingEnvironment*/)
        {
            //_hostingEnvironment = hostingEnvironment;
        }
        public List<Models.ResourceBaseModel> GetResourceBaseData()
        {
            var objBaseList = new List<Models.ResourceBaseModel>();
            var languages = new LocalizationLanguages();
            var resources = new ResourceModules();
            string binFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            foreach (var module in resources.Modules)
            {
                var objModule = new ResourceModel();
                string filePath = Path.Combine(binFolderPath + "\\DBResources", $"{module.ResourceFile}.json");
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        var json = reader.ReadToEnd();
                        Models.ResourceModel resource = JsonConvert.DeserializeObject<Models.ResourceModel>(json);
                        objModule = resource;
                    }
                }

                if (objModule != null && objModule.Translations != null)
                {
                    foreach (var transalation in objModule.Translations)
                    {
                        foreach (var language in languages.Languages)
                        {
                            var objBase = new Models.ResourceBaseModel();
                            objBase.ModuleCode = module.ModuleCode;
                            objBase.ModuleName = module.ModuleName;
                            objBase.LanguageCode = language.Culture;
                            objBase.LanguageName = language.Name;

                            objBase.Name = transalation.Name;
                            objBase.Description = transalation.Description;

                            var languageLocalization = transalation.Localizations?.Where(c => c.Culture == language.Culture).FirstOrDefault();
                            if (languageLocalization != null)
                            {
                                objBase.Value = languageLocalization.Value;
                            }
                            objBaseList.Add(objBase);
                        }
                    }
                }

            }
            return objBaseList;
        }
    }
}
