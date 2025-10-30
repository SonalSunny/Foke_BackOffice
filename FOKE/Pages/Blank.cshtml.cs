using FOKE.Localization.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FOKE.Pages
{
    public class BlankModel : PageModel
    {
        private IResourceManagerService _webHostEnvironment { get; set; }
        public BlankModel(IResourceManagerService webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {

            var a = _webHostEnvironment.GetResourceBaseData();
        }
    }
}
