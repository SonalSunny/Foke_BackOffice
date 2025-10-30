using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FOKE.Pages.CommitteManagement
{
    public class ViewGroupModel : PageModel
    {

        private readonly ICommitteGroupRepository _committeeGroupRepository;

        public CommitteGroupViewModel CommitteGroup { get; set; }

        public ViewGroupModel(ICommitteGroupRepository committeeGroupRepository)
        {
            _committeeGroupRepository = committeeGroupRepository;
        }

        public void OnGet(long id)
        {
            var retData = _committeeGroupRepository.GetCommitteeGroupById(id);
            if (retData.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                CommitteGroup = retData.returnData;
            }
            else
            {

                CommitteGroup = new CommitteGroupViewModel();
            }
        }
    }
}
