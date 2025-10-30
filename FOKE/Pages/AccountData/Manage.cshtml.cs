using FOKE.Entity;
using FOKE.Entity.AccountsData.ViewModel;
using FOKE.Entity.Common;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.AccountData
{
    public class ManageModel : PagedListBasePageModel
    {
        [BindProperty]
        public AccountViewModel inputModel { get; set; }

        public string? pageErrorMessage { get; set; }

        public readonly IAccountsRepostory _accountsRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public List<DropDownViewModel> AccountTypeList { get; set; }
        public List<DropDownViewModel> CategoryTypeList { get; set; }

        public ManageModel(IAccountsRepostory accountsRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _accountsRepository = accountsRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            BindDropdowns();
            inputModel = new AccountViewModel();
            if (id > 0)
            {
                var retData = _accountsRepository.GetAccountDataById(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            else
            {
                _formMode = FormModeEnum.add.ToString();
            }
            setPagedListColumns();
        }

        public async Task<IActionResult> OnPost()
        {
            var Username = inputModel.TotalAmount;
            if (Username == null)
            {
                pageErrorMessage = "Please enter Complete Details";
            }
            else
            {
                var retData = new ResponseEntity<AccountViewModel>();
                if (btnSubmit == "btnSave")
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _accountsRepository.AddIncomeExpense(inputModel);
                        if (retData.transactionStatus != HttpStatusCode.OK)
                        {
                            pageErrorMessage = retData.returnMessage;
                            IsSuccessReturn = false;
                        }
                        else
                        {
                            ModelState.Clear();
                            IsSuccessReturn = true;
                            sucessMessage = retData.returnMessage;
                            inputModel = new AccountViewModel();
                            BindDropdowns();
                            return Page();
                        }
                    }
                    else
                    {

                        retData = await _accountsRepository.UpdateIncomeExpense(inputModel);
                        if (retData.transactionStatus != HttpStatusCode.OK)
                        {
                            pageErrorMessage = retData.returnMessage;
                            IsSuccessReturn = false;
                        }
                        else
                        {
                            ModelState.Clear();
                            IsSuccessReturn = true;
                            sucessMessage = retData.returnMessage;
                        }
                    }
                }
                else
                {
                    if (btnSubmit == "btnSave")
                    {
                        retData.transactionStatus = HttpStatusCode.BadRequest;
                        pageErrorMessage = "Use 8 or more characters with a mix of letters,numbers,symbols.";
                        IsSuccessReturn = false;
                    }
                    else
                    {
                        ModelState.Clear();
                    }
                }
            }
            BindDropdowns();
            return Page();
        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FullName", ColumnDescription = _sharedLocalizer.Localize("Full Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilID", ColumnDescription = _sharedLocalizer.Localize("Civil ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Passport Number", ColumnDescription = _sharedLocalizer.Localize("Passport Number").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Contact Number", ColumnDescription = _sharedLocalizer.Localize("Contact Number").Value });

            pageListFilterColumns = objList;
        }
        private void BindDropdowns()
        {
            AccountTypeList = _dropDownRepository.GetAccountTypes();
            CategoryTypeList = _dropDownRepository.GetCategoryTypes();
        }
    }
}
