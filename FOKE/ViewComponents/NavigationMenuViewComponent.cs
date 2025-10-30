using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FOKE.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IMenuRepository _menuRepository;
        public NavigationMenuViewComponent(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _menuRepository.GetApplicationMenusBygroup(HttpContext.User);
            return View(items);
        }
    }
}
