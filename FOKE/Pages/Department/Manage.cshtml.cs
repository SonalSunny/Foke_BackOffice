using FOKE.Entity;
using FOKE.Entity.DepartmentMaster.ViewModel;
//using FOKE.Entity.Department.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Department
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public DepartmentViewModel inputModel { get; set; }

        private readonly IDepartmentRepository _departmentRepository;
        public string? pageErrorMessage { get; set; }
        public ManageModel(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;


        }
        public void OnGet(long? id, string mode)
        {

            _formMode = mode;
            isValidRequest = true;
            inputModel = new DepartmentViewModel();
            if (id > 0)
            {

                var retData = _departmentRepository.GetDepartmentbyId(Convert.ToInt64(id));
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

        }










        public async Task<IActionResult> OnPost()
        {
            var dept = inputModel.DepartmentName;
            var description = inputModel.Description;

            if (dept == null)
            {
                pageErrorMessage = "Enter Department";
                return Page();
            }
            else
            {
                var retData = new ResponseEntity<DepartmentViewModel>();

                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _departmentRepository.AddDepartment(inputModel);

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
                            inputModel = new DepartmentViewModel();

                            return Page();
                        }
                    }
                    else

                    {
                        retData = await _departmentRepository.UpdateDepartment(inputModel);
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
                            inputModel = new DepartmentViewModel();
                        }
                    }
                }
                return Page();
            }
        }
    }
}
