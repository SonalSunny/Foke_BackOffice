using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace FOKE.Localization.Services
{
    public class LanguageDBService
    {

        [RazorInject]
        public ILocalizationService LocalizationService { get; set; }

        public delegate HtmlString Localizer(string resourceKey, params object[] args);
        private Localizer _localizer;

        public Localizer Localize
        {
            get
            {
                if (_localizer == null)
                {
                    var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

                    //var language = LanguageService.GetLanguageByCulture(currentCulture);
                    //if (language != null)
                    //{
                    //    _localizer = (resourceKey, args) =>
                    //    {
                    //        var stringResource = LocalizationService.GetStringResource(resourceKey, language.LanguageId);

                    //        if (stringResource == null || string.IsNullOrEmpty(stringResource.Value))
                    //        {
                    //            return new HtmlString(resourceKey);
                    //        }

                    //        return new HtmlString((args == null || args.Length == 0)
                    //            ? stringResource.Value
                    //            : string.Format(stringResource.Value, args));
                    //    };
                    //}
                }
                return _localizer;
            }
        }
    }
}
