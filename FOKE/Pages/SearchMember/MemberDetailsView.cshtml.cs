using FOKE.Localization;
using FOKE.Pages.IssueMembership.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace FOKE.Pages.SearchMember
{
    public class MemberDetailsViewModel : PageModel
    {

        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<IssueMemberShipViewModel> pagedListData { get; private set; }
        public void OnGet()
        {
        }
    }
}
