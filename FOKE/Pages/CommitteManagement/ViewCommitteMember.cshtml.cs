using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace FOKE.Pages.CommitteManagement
{
    public class ViewCommitteMemberModel : PageModel
    {
        private readonly ICommitteeMemberRepository _repo;

        public ViewCommitteMemberModel(ICommitteeMemberRepository repo)
        {
            _repo = repo;
        }

        public CommitteMemberViewModel Member { get; set; }

        public IActionResult OnGet(long id)
        {
            var res = _repo.GetCommitteeMemberById(id);
            if (res.transactionStatus == HttpStatusCode.OK)
            {
                Member = res.returnData;
                return Page();
            }
            return NotFound();
        }
    }
}
