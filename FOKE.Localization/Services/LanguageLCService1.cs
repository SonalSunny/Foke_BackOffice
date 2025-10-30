
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace FOKE.Localization.Services
{
    public class SharedResource
    {
    }
    public class ValidationResource
    {
    }
    public class LanguageLCService
    {
        private readonly IStringLocalizer _localizer;
        private readonly IStringLocalizer _Menulocalizer;


        public LanguageLCService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
            _Menulocalizer = factory.Create("MenuResource", assemblyName.Name);
        }

        public LocalizedString Getkey(string key)
        {
            return _localizer[key];
        }

        public LocalizedString GetMenukey(string key)
        {
            return _Menulocalizer[key];
        }

        public string GetCurrentCulture()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

            return currentCulture ?? "en-US";
        }

        public string GetLayOutDirection()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

            var culture = currentCulture ?? "en-US";


            if (culture == "ar-AE")
            {
                return "rtl";
            }
            else
            {
                return "ltl";
            }
            ;
        }
    }
}
