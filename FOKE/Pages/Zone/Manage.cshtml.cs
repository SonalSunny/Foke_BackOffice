using FOKE.Entity;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.ZoneMaster.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Zone
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public ZoneViewModel inputModel { get; set; }

        private readonly IZoneRepository _zoneRepository;
        private readonly IUserRepository _userRepository;
        public string? pageErrorMessage { get; set; }
        public long? _zoneId { get; set; }
        public ManageModel(IZoneRepository zoneRepository, IUserRepository userRepository)
        {
            _zoneRepository = zoneRepository;
            _userRepository = userRepository;
        }

        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new ZoneViewModel();

            // 1. If editing an existing area
            if (id > 0)
            {
                _zoneId = id;
                var retData = _zoneRepository.GetZonebyId(id.Value);
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                    // ✅ 2. Load assigned user IDs for checkboxes
                    inputModel.AssignedUserIds = _zoneRepository
                        .GetMembersAssignedToZone(id.Value)
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
        //    inputModel = new ZoneViewModel();
        //    if (id > 0)
        //    {
        //        _zoneId = id;

        //        var retData = _zoneRepository.GetZonebyId(Convert.ToInt64(id));
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
            var zoneName = inputModel.ZoneName;
            var description = inputModel.Description;

            if (zoneName == null)
            {
                pageErrorMessage = "Enter Zone";
                return Page();
            }
            else
            {
                var retData = new ResponseEntity<ZoneViewModel>();

                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _zoneRepository.AddZone(inputModel);

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
                            inputModel = new ZoneViewModel();

                            return Page();
                        }
                    }
                    else

                    {
                        retData = await _zoneRepository.UpdateZone(inputModel);
                        if (retData.transactionStatus != HttpStatusCode.OK)
                        {
                            pageErrorMessage = retData.returnMessage;
                            IsSuccessReturn = false;
                        }
                        else
                        {

                            await _zoneRepository.UpdateAssignedUsers(inputModel.ZoneId.Value, inputModel.AssignedUserIds, inputModel.loggedinUserId ?? 0);
                            ModelState.Clear();
                            IsSuccessReturn = true;
                            sucessMessage = retData.returnMessage;
                            inputModel = new ZoneViewModel();
                        }
                    }
                }
                return Page();
            }
        }


    }
}
