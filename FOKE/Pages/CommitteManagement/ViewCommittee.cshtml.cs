using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FOKE.Pages.CommitteManagement
{
    public class ViewCommitteeModel : PageModel
    {
        private readonly ICommitteeRepository _committeeRepository;

        public CommitteViewModel Committe { get; set; }

        public ViewCommitteeModel(ICommitteeRepository committeeRepository)
        {
            _committeeRepository = committeeRepository;
        }

        public void OnGet(long id)
        {
            var retData = _committeeRepository.GetCommitteeById(id);
            if (retData.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                Committe = retData.returnData;
            }
            else
            {

                Committe = new CommitteViewModel();
            }
        }
    }
}
