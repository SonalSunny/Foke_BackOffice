using FOKE.Entity;
using FOKE.Entity.AccountsData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IAccountsRepostory
    {
        Task<ResponseEntity<AccountViewModel>> AddIncomeExpense(AccountViewModel model);
        Task<ResponseEntity<AccountViewModel>> UpdateIncomeExpense(AccountViewModel model);
        ResponseEntity<bool> DeleteAccountData(AccountViewModel objModel);
        ResponseEntity<List<AccountViewModel>> GetAllAccountsData(long? Status);
        ResponseEntity<AccountViewModel> GetAccountDataById(long AccountId);
    }
}
