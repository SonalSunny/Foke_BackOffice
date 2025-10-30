using FOKE.DataAccess;
using FOKE.Entity.Common;


using FOKE.Models.PageModels;
using FOKE.Pages.IssueMembership.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FOKE.Pages.MembersList.AllMembersList
{
    public class MemberViewModel : BasePageModel
    {
        public IssueMemberShipViewModel inputModel { get; set; }

        public List<IssueMemberShipViewModel> Comments { get; set; }

        [BindProperty]
        public int TaskId { get; set; }

        [BindProperty]
        public int CommentType { get; set; }

        [BindProperty]
        public string CommentHtml { get; set; }

        [BindProperty]
        public IFormFile Attachment { get; set; }

        public long? Status { get; set; }
        public List<DropDownViewModel> StatusList { get; set; }

        private readonly FOKEDBContext _dbContext;
        private readonly IDropDownRepository _dropDownRepository;
        public string? pageErrorMessage { get; set; }

        public MemberViewModel(IDropDownRepository dropDownRepository)
        {
            _dropDownRepository = dropDownRepository;
        }

        public void OnGet(long? id)
        {
            inputModel = new IssueMemberShipViewModel();
            if (id > 0)
            {

            }
            else
            {
            }
        }

    }
}
