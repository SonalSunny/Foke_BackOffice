using FOKE.Entity;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.UnitData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Unit
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public UnitViewModel inputModel { get; set; }

        private readonly IUnitRepository _unitRepository;
        private readonly IUserRepository _userRepository;

        public string? pageErrorMessage { get; set; }
        public long? _unitId { get; set; }
        public ManageModel(IUnitRepository unitRepository, IUserRepository userRepository)
        {
            _unitRepository = unitRepository;
            _userRepository = userRepository;

        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new UnitViewModel();

            // 1. If editing an existing area
            if (id > 0)
            {
                _unitId = id;
                var retData = _unitRepository.GetUnitbyId(id.Value);
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                    // ✅ 2. Load assigned user IDs for checkboxes
                    inputModel.AssignedUserIds = _unitRepository
                        .GetMembersAssignedToUnit(id.Value)
                        .Select(u => u.UserId)
                        .ToList();
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }

            // ✅ 3. Load all users to display in checkbox list
            var allUsersResult = _userRepository.GetAllUsers(1, null, null, null);
            if (allUsersResult.transactionStatus == HttpStatusCode.OK)
            {
                inputModel.AllUsers = allUsersResult.returnData
                    .Select(u => new Users
                    {
                        UserId = u.UserId,
                        UserName = u.Username
                    }).ToList();
            }
        }

        //public void OnGet(long? id, string mode)
        //{
        //    _formMode = mode;
        //    isValidRequest = true;
        //    inputModel = new UnitViewModel();
        //    if (id > 0)
        //    {
        //        _unitId = id;
        //        var retData = _unitRepository.GetUnitbyId(Convert.ToInt64(id));
        //        if (retData.transactionStatus == HttpStatusCode.OK)
        //        {
        //            isValidRequest = true;
        //            inputModel = retData.returnData;

        //        }
        //        else
        //        {
        //            isValidRequest = false;
        //            pageErrorMessage = retData.returnMessage;
        //        }

        //    }


        //}

        public async Task<IActionResult> OnPost()
        {
            var unitname = inputModel.UnitName;
            //   var description = inputModel.Description;

            if (unitname == null)
            {
                pageErrorMessage = "Enter Unit";
                return Page();
            }
            else
            {
                var retData = new ResponseEntity<UnitViewModel>();

                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _unitRepository.AddUnit(inputModel);

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
                            inputModel = new UnitViewModel();

                            return Page();
                        }
                    }
                    else

                    {
                        retData = await _unitRepository.UpdateUnit(inputModel);
                        if (retData.transactionStatus != HttpStatusCode.OK)
                        {
                            pageErrorMessage = retData.returnMessage;
                            IsSuccessReturn = false;
                        }
                        else
                        {
                            await _unitRepository.UpdateAssignedUsers(inputModel.UnitId, inputModel.AssignedUserIds, inputModel.loggedinUserId ?? 0);


                            ModelState.Clear();
                            IsSuccessReturn = true;
                            sucessMessage = retData.returnMessage;
                            inputModel = new UnitViewModel();
                        }
                    }
                }
                return Page();
            }
        }

    }
}
