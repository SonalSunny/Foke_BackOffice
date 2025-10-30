using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.AccountsData.DTO;
using FOKE.Entity.AccountsData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class AccountsRepostory : IAccountsRepostory
    {

        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentRepository _attachmentRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public AccountsRepostory(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachmentRepository)
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
            catch (Exception ex)
            {
            }

            _attachmentRepository = attachmentRepository;
        }

        public async Task<ResponseEntity<AccountViewModel>> AddIncomeExpense(AccountViewModel model)
        {
            var retModel = new ResponseEntity<AccountViewModel>();
            try
            {

                if (model == null || model.TotalAmount == null)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Invalid input parameters.";
                    return retModel;
                }
                else
                {
                    var AccountData = new Account
                    {
                        Category = model.Category,
                        CategoryType = model.CategoryType,
                        Date = model.Date,
                        TotalAmount = model.TotalAmount,
                        RefNo = model.RefNo,
                        Remarks = model.Remarks,
                        Active = true,
                        CreatedBy = loggedInUser,//loginned user employee is
                        CreatedDate = DateTime.UtcNow,
                    };
                    await _dbContext.Accounts.AddAsync(AccountData);
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Added Successfully";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public async Task<ResponseEntity<AccountViewModel>> UpdateIncomeExpense(AccountViewModel model)
        {
            var retModel = new ResponseEntity<AccountViewModel>();
            try
            {
                var AccountData = _dbContext.Accounts.Where(c => c.Id == model.Id && c.Active == true).FirstOrDefault();
                if (AccountData != null)
                {
                    AccountData.Category = model.Category;
                    AccountData.TotalAmount = model.TotalAmount;
                    AccountData.Date = model.Date;
                    AccountData.RefNo = model.RefNo;
                    AccountData.TotalAmount = model.TotalAmount;
                    AccountData.Remarks = model.Remarks;
                    AccountData.UpdatedDate = DateTime.UtcNow;
                    AccountData.UpdatedBy = loggedInUser;

                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Updated Successfully";
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Data does not exist";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public ResponseEntity<bool> DeleteAccountData(AccountViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var AccountsData = _dbContext.Accounts.Find(objModel.Id);
                if (objModel.DiffId == 1)
                {
                    AccountsData.Active = false;
                    _dbContext.Entry(AccountsData).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    AccountsData.Active = true;
                    _dbContext.Entry(AccountsData).State = EntityState.Modified;
                    _dbContext.SaveChanges();
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

        public ResponseEntity<List<AccountViewModel>> GetAllAccountsData(long? Status)
        {
            var retModel = new ResponseEntity<List<AccountViewModel>>();
            try
            {
                var objModel = new List<AccountViewModel>();

                var retData = _dbContext.Accounts.ToList();

                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = retData.Where(c => c.Active == true || c.Active == false).ToList();
                    }
                    else if (Status == 1)
                    {
                        retData = retData.Where(c => c.Active).ToList();
                    }
                    else if (Status == 0)
                    {
                        retData = retData.Where(c => !c.Active).ToList();
                    }
                }

                objModel = retData.Select(c => new AccountViewModel()
                {
                    Id = c.Id,
                    Category = c.Category,
                    CategoryName = c.Category != null ? _dbContext.LookupMasters.FirstOrDefault(l => l.LookUpId == c.Category).LookUpName : null,
                    TotalAmount = c.TotalAmount,
                    Date = c.Date,
                    RefNo = c.RefNo,
                    Remarks = c.Remarks,
                    Active = c.Active,
                    CreatedDate = c.CreatedDate,
                    CreatedBy = _dbContext.Users
                        .Where(u => u.UserId == c.CreatedBy)
                        .Select(u => u.UserId)
                        .FirstOrDefault(),
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderByDescending(i => i.Id).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<AccountViewModel> GetAccountDataById(long AccountId)
        {
            var retModel = new ResponseEntity<AccountViewModel>();
            try
            {
                var AccountData = _dbContext.Accounts.SingleOrDefault(u => u.Id == AccountId);
                var objModel = new AccountViewModel();
                objModel.Id = AccountData.Id;
                objModel.Category = AccountData.Category;
                objModel.TotalAmount = AccountData.TotalAmount;
                objModel.Date = AccountData.Date;
                objModel.RefNo = AccountData.RefNo;
                objModel.Remarks = AccountData.Remarks;
                objModel.Active = AccountData.Active;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        //public ResponseEntity<string> ExportUserDatatoExcel(string search, long? statusid)
        //{
        //    var retModel = new ResponseEntity<string>();
        //    try
        //    {
        //        var objData = GetAllOfferData(statusid);

        //        if (objData.transactionStatus == HttpStatusCode.OK)
        //        {
        //            using (var workbook = new XLWorkbook())
        //            {
        //                var worksheet = workbook.Worksheets.Add("Offers For Members");
        //                worksheet.Cell(1, 1).Value = "Sl No";
        //                worksheet.Cell(1, 2).Value = "Offer";
        //                worksheet.Cell(1, 3).Value = "Description";
        //                worksheet.Cell(1, 4).Value = "Show in Website";
        //                worksheet.Cell(1, 5).Value = "Show in Mobile";
        //                worksheet.Cell(1, 6).Value = "Created By";
        //                worksheet.Cell(1, 7).Value = "Created Date";
        //                worksheet.Cell(1, 8).Value = "Status";

        //                var headerRow = worksheet.Row(1);
        //                headerRow.Style.Font.Bold = true;
        //                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

        //                for (int i = 0; i < objData.returnData.Count; i++)
        //                {
        //                    worksheet.Cell(i + 2, 1).Value = i + 1;
        //                    worksheet.Cell(i + 2, 2).Value = objData.returnData[i].Heading;
        //                    worksheet.Cell(i + 2, 3).Value = objData.returnData[i].Description;
        //                    if (objData.returnData[i].ShowInWebsite)
        //                    {
        //                        worksheet.Cell(i + 2, 4).Value = "Yes";
        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, 4).Value = "No";
        //                    }
        //                    if (objData.returnData[i].ShowInMobile)
        //                    {
        //                        worksheet.Cell(i + 2, 5).Value = "Yes";
        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, 5).Value = "No";
        //                    }
        //                    worksheet.Cell(i + 2, 7).Value = objData.returnData[i].CreatedUsername;
        //                    worksheet.Cell(i + 2, 8).Value = objData.returnData[i].CreatedDate;
        //                    if (objData.returnData[i].Active)
        //                    {
        //                        worksheet.Cell(i + 2, 9).Value = "Active";
        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, 9).Value = "Inactive";
        //                    }
        //                }

        //                using (var stream = new MemoryStream())
        //                {
        //                    workbook.SaveAs(stream);
        //                    stream.Position = 0;
        //                    byte[] fileBytes = stream.ToArray();
        //                    retModel.returnData = GenericUtilities.SetReportData(fileBytes, ".xlsx");
        //                    retModel.transactionStatus = HttpStatusCode.OK;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retModel.transactionStatus = HttpStatusCode.InternalServerError;
        //    }
        //    return retModel;
        //}

    }
}
