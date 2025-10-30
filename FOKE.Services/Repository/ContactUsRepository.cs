using ClosedXML.Excel;
using FirebaseAdmin.Messaging;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.ContactUs.DTO;
using FOKE.Entity.ContactUs.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class ContactUsRepository : IcontactUsRepository
    {

        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public ContactUsRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;

            try
            {
                claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
                var isAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false;
                if (isAuthenticated)
                {
                    var userIdentity = claimsPrincipal?.Identity?.Name;
                    if (userIdentity != null)
                    {
                        long userid = 0;
                        Int64.TryParse(userIdentity, out userid);
                        if (userid > 0)
                        {
                            loggedInUser = userid;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task<ResponseEntity<bool>> AddClientEnquiery(ContactUsDto model)
        {
            var retModel = new ResponseEntity<bool>();
            retModel.returnData = false;
            try
            {
                var contactusData = new ClientEnquieryData
                {
                    Name = model.Name,
                    Description = model.Description,
                    ContactNo = model.ContactNo,
                    DevicePrimaryId = model.DevicePrimaryId,
                    DeviceId = model.DeviceId,
                    CivilId = model.CivilId,
                    Type = model.Type,
                    Active = true,
                    CreatedDate = DateTime.UtcNow,
                };
                await _dbContext.ClientEnquieryDatas.AddAsync(contactusData);
                await _dbContext.SaveChangesAsync();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Saved Successfully";
                retModel.returnData = true;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error Occured";
            }
            return retModel;
        }

        public async Task<ResponseEntity<List<ClientEnquieryViewModel>>> GetAllClientEnquiery(long? StatusId)
        {
            var ReturnData = new ResponseEntity<List<ClientEnquieryViewModel>>();
            try
            {
                var Data = _dbContext.ClientEnquieryDatas.ToList();
                if (Data.Any())
                {
                    if (StatusId.HasValue)
                    {
                        if (StatusId == 2)
                        {
                            Data = Data.Where(c => c.Active == true || c.Active == false).ToList();
                        }
                        else if (StatusId == 1)
                        {
                            Data = Data.Where(c => c.Active).ToList();
                        }
                        else if (StatusId == 0)
                        {
                            Data = Data.Where(c => !c.Active).ToList();
                        }
                    }
                    var ClientData = Data.Select(i => new ClientEnquieryViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        ContactNo = i.ContactNo,
                        Type = i.Type != null ? i.Type == 1 ? "Login Issue" : "In-App Issue" : null,
                        Active = i.Active,
                        DateOfCreation = i.CreatedDate != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.CreatedDate, GenericUtilities.dateTimeFormat) : "N/A",
                        ResolvedBy = i.ResolvedBy != null ? _dbContext.Users.Find(i.ResolvedBy).UserName : "N/A",
                        ResolvedDate = i.ResolvedDate != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.ResolvedDate, GenericUtilities.dateTimeFormat) : "N/A",
                        IsResolved = i.IsResolved,
                    }).ToList();
                    ReturnData.returnData = ClientData.OrderByDescending(i => i.Id).ToList();
                    ReturnData.transactionStatus = System.Net.HttpStatusCode.OK;
                    ReturnData.returnMessage = "Success";
                }
                else
                {
                    ReturnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    ReturnData.returnMessage = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                ReturnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                ReturnData.returnMessage = "Server Error Occured";
            }
            return ReturnData;
        }

        public async Task<ResponseEntity<bool>> DeleteEnquiery(ClientEnquieryViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var Offerdata = _dbContext.ClientEnquieryDatas.Find(objModel.Id);
                if (objModel.DiffId == 1)
                {
                    Offerdata.Active = false;
                    _dbContext.Entry(Offerdata).State = EntityState.Modified;
                    _dbContext.SaveChangesAsync();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    Offerdata.Active = true;
                    _dbContext.Entry(Offerdata).State = EntityState.Modified;
                    _dbContext.SaveChangesAsync();
                    retModel.returnMessage = "Activated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public async Task<ResponseEntity<string>> ExportClientRequestDatatoExcel(string search, long? statusid)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = await GetAllClientEnquiery(statusid);
                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Client Enquieries");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Name";
                        worksheet.Cell(1, 3).Value = "Message";
                        worksheet.Cell(1, 4).Value = "PhoneNo";
                        worksheet.Cell(1, 5).Value = "Created Date";
                        worksheet.Cell(1, 6).Value = "Status";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].Name;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].Description;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].ContactNo;
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].DateOfCreation;
                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 6).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 6).Value = "Inactive";
                            }
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.Position = 0;
                            byte[] fileBytes = stream.ToArray();
                            retModel.returnData = GenericUtilities.SetReportData(fileBytes, ".xlsx");
                            retModel.transactionStatus = HttpStatusCode.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public async Task<ResponseEntity<bool>> ResolveIssue(long? Id, string Comment)
        {
            var returndata = new ResponseEntity<bool>();
            try
            {
                var clientEnquieryData = _dbContext.ClientEnquieryDatas.FirstOrDefault(i => i.Id == Id);
                var DeviceDetails = clientEnquieryData != null ? _dbContext.DeviceDetails.FirstOrDefault(i => i.DeviceDetailId == clientEnquieryData.DevicePrimaryId) : null;
                if (DeviceDetails != null)
                {
                    clientEnquieryData.Comment = Comment;

                    var message = new Message
                    {
                        Token = DeviceDetails.FCMToken,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = "Attention",
                            Body = Comment,
                        }
                    };
                    var firebaseResponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    var FirebaseSuccess = true;
                    if (FirebaseSuccess)
                    {
                        clientEnquieryData.IsNotificationSent = true;
                        clientEnquieryData.IsResolved = true;
                        _dbContext.SaveChangesAsync();
                        returndata.returnData = true;
                        returndata.transactionStatus = System.Net.HttpStatusCode.OK;
                        returndata.returnMessage = "Success";
                    }
                    else
                    {
                        returndata.transactionStatus = System.Net.HttpStatusCode.Conflict;
                        returndata.returnMessage = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                returndata.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                returndata.returnMessage = "Server Error Occured";
            }
            return returndata;
        }
    }
}
