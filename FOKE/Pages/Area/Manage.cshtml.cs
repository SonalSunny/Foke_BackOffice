using FOKE.Entity;
using FOKE.Entity.AreaMaster.ViewModel;
using FOKE.Entity.Identity.DTO;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Area
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public AreaDataViewModel inputModel { get; set; }

        private readonly IAreaRepository _areaRepository;
        private readonly IUserRepository _userRepository;
        public string? pageErrorMessage { get; set; }
        public long? _areaId { get; set; }
        public ManageModel(IAreaRepository areaRepository, IUserRepository userRepository)
        {
            _areaRepository = areaRepository;
            _userRepository = userRepository;

        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new AreaDataViewModel();

            // 1. If editing an existing area
            if (id > 0)
            {
                _areaId = id;
                var retData = _areaRepository.GetAreabyId(id.Value);
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                    // ✅ 2. Load assigned user IDs for checkboxes
                    inputModel.AssignedUserIds = _areaRepository
                        .GetMembersAssignedToArea(id.Value)
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
        //    inputModel = new AreaDataViewModel();
        //    if (id > 0)
        //    {
        //        _areaId = id;
        //        var retData = _areaRepository.GetAreabyId(Convert.ToInt64(id));
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
            var areaname = inputModel.AreaName;
            var description = inputModel.Description;

            if (areaname == null)
            {
                pageErrorMessage = "Enter Area";
                return Page();
            }
            else
            {
                var retData = new ResponseEntity<AreaDataViewModel>();

                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _areaRepository.AddArea(inputModel);

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
                            inputModel = new AreaDataViewModel();

                            return Page();
                        }
                    }
                    else

                    {
                        retData = await _areaRepository.UpdateArea(inputModel);
                        if (retData.transactionStatus != HttpStatusCode.OK)
                        {
                            pageErrorMessage = retData.returnMessage;
                            IsSuccessReturn = false;
                        }
                        else
                        {
                            await _areaRepository.UpdateAssignedUsers(inputModel.AreaId.Value, inputModel.AssignedUserIds, inputModel.loggedinUserId ?? 0);

                            ModelState.Clear();
                            IsSuccessReturn = true;
                            sucessMessage = retData.returnMessage;
                            inputModel = new AreaDataViewModel();
                        }
                    }
                }
                return Page();
            }
        }


    }
}
