//using DocumentFormat.OpenXml.Wordprocessing;
using FOKE.Localization;
using FOKE.Localization.Models;
using FOKE.Localization.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;

namespace FOKE.Startup
{
    public static class LocalizationInjector
    {
        public static void RegisterLocalization(this IServiceCollection services)
        {
            #region Localization
            //Step 1

            services.AddSingleton<LanguageLCService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ISharedLocalizer, SharedLocalizer>();
            services.AddScoped<IResourceManagerService, ResourceManagerService>();

            //services.AddScoped<ISharedLocalizer, SharedLocalizer>();
            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.AddMvc()
            //	.AddViewLocalization()
            //	.AddDataAnnotationsLocalization(options =>
            //	{
            //		options.DataAnnotationLocalizerProvider = (type, factory) =>
            //		{
            //			var assemblyName = new AssemblyName(typeof(ValidationResource).GetTypeInfo().Assembly.FullName);
            //			return factory.Create("ValidationResource", assemblyName.Name);
            //		};
            //	});

            //services.Configure<RequestLocalizationOptions>(
            //	options =>
            //	{
            //		var supportedCultures = new List<CultureInfo>
            //			{							
            //				new CultureInfo("en-US"),
            //				new CultureInfo("ar-AE"),
            //			};

            //		options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            //		options.SupportedCultures = supportedCultures;
            //		options.SupportedUICultures = supportedCultures;

            //		options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            //	});

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(ValidationResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ValidationResource", assemblyName.Name);
                    };
                });

            var serviceProvider = services.BuildServiceProvider();

            //var languageService = serviceProvider.GetRequiredService<ILanguageService>();
            //var languages = languageService.GetLanguages();
            //var cultures = languages.Select(x => new CultureInfo(x.Culture)).ToArray();

            var languages = new LocalizationLanguages();
            var cultures = languages.Languages.Select(x => new CultureInfo(x.Culture)).ToArray();
            services.Configure<RequestLocalizationOptions>(
                    options =>
                    {
                        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                        options.SupportedCultures = cultures;
                        options.SupportedUICultures = cultures;

                        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                    });

            #endregion

        }
    }
}
