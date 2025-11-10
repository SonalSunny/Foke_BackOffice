using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.MembershipData.DTO;
using FOKE.Entity.MembershipFee.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        private readonly IFeeCollectionReport _feeCollectionReport;
        public CampaignRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IFeeCollectionReport feeCollectionReport)
        {
            this._dbContext = FOKEDBContext;
            this._httpContextAccessor = httpContextAccessor;
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

            _feeCollectionReport = feeCollectionReport;
        }
        public async Task<ResponseEntity<CampaignViewModel>> AddCampaign(CampaignViewModel model)
        {
            var retModel = new ResponseEntity<CampaignViewModel>();

            try
            {

                var CampaignExists = _dbContext.Campaigns
                       .Any(u => u.CampaignName == model.CampaignName);
                if (CampaignExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Campaign Already Exists";
                }
                else
                {


                    var Campaign = new Entity.CampaignData.DTO.Campaign
                    {
                        CampaignName = model.CampaignName,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        MemberShipFee = model.MemberShipFee,
                        Active = false,
                        Description = model.Description

                    };
                    await _dbContext.Campaigns.AddAsync(Campaign);
                    await _dbContext.SaveChangesAsync();

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Saved Successfully";
                    retModel.returnData = model;

                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error Occured";
            }
            return retModel;
        }


        public async Task<ResponseEntity<CampaignViewModel>> UpdateCampaign(CampaignViewModel model)
        {
            var retModel = new ResponseEntity<CampaignViewModel>();

            try
            {
                var Campaign = await _dbContext.Campaigns
                    .Where(r => r.CampaignId == model.CampaignId)
                    .SingleOrDefaultAsync();

                if (Campaign != null)
                {
                    // Checking if the profession already exists
                    var CampaignExists = await _dbContext.Campaigns
                        .AnyAsync(r => r.CampaignId != model.CampaignId && r.CampaignName == model.CampaignName && r.Active);

                    if (CampaignExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Campaign already exists with the same name";
                        return retModel;
                    }

                    Campaign.CampaignName = model.CampaignName;
                    Campaign.StartDate = model.StartDate;
                    Campaign.EndDate = model.EndDate;
                    Campaign.MemberShipFee = model.MemberShipFee;

                    Campaign.Description = model.Description;
                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Campaign updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Campaign not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }




        public ResponseEntity<CampaignViewModel> GetCampaignId(long CampaignId)
        {
            var retModel = new ResponseEntity<CampaignViewModel>();
            try
            {
                var objRole = _dbContext.Campaigns
                     .SingleOrDefault(u => u.CampaignId == CampaignId);
                var objModel = new CampaignViewModel();
                objModel.CampaignId = objRole.CampaignId;
                objModel.CampaignName = objRole.CampaignName;
                objModel.StartDate = objRole.StartDate;
                objModel.EndDate = objRole.EndDate;
                objModel.MemberShipFee = objRole.MemberShipFee;
                objModel.Description = objRole.Description;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<List<CampaignViewModel>> GetAllCampaign(long? Status)
        {
            var retModel = new ResponseEntity<List<CampaignViewModel>>();
            try
            {
                var objModel = new List<CampaignViewModel>();

                IQueryable<Entity.CampaignData.DTO.Campaign> retData = _dbContext.Campaigns.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.Campaigns.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.Campaigns.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.Campaigns.Where(c => c.Active == false);
                    }
                }

                objModel = retData.Select(c => new CampaignViewModel()
                {
                    CampaignId = c.CampaignId,
                    CampaignName = c.CampaignName,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    MemberShipFee = c.MemberShipFee,
                    CollectionAdded = c.CollectionAdded ?? 0,
                    Active = c.Active,
                    Description = c.Description
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }
        public async Task<bool> CreateCollectionSheetAsync(long campaignId)
        {
            var now = DateTime.UtcNow;

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var selectedCampaign = await _dbContext.Campaigns
                    .FirstOrDefaultAsync(c => c.CampaignId == campaignId);

                if (selectedCampaign == null || selectedCampaign.CollectionAdded == 1)
                    return false;

                var activeCampaigns = await _dbContext.Campaigns
                    .Where(c => c.Active && c.CampaignId != campaignId)
                    .ToListAsync();

                if (activeCampaigns.Count >= 2)
                    return false;


                var members = await _dbContext.MembershipAcceptedDatas.ToListAsync();
                foreach (var member in members)
                {
                    bool hasPaid = member.CampaignId == campaignId && member.AmountRecieved > 0;


                    bool feeAlreadyExists = await _dbContext.MembershipFees
                        .AnyAsync(f => f.MemberID == member.IssueId && f.Campaign == campaignId);

                    if (feeAlreadyExists)
                        continue;

                    var feeEntry = new MembershipFee
                    {
                        MemberID = member.IssueId,
                        Campaign = campaignId,
                        AmountToPay = selectedCampaign.MemberShipFee ?? 0,
                        PaidAmount = hasPaid ? member.AmountRecieved : 0,
                        PaidDate = hasPaid ? member.PaidDate : null,
                        PaymentType = hasPaid ? member.PaymentTypeId : null,
                        PaymentReceivedBy = hasPaid ? member.PaymentReceivedBy : null,
                        CollectionRemark = member.PaymentRemarks,
                        Active = true,
                        CreatedBy = loggedInUser,
                        CreatedDate = now
                    };

                    _dbContext.MembershipFees.Add(feeEntry);


                    if (!hasPaid)
                    {
                        member.CampaignId = campaignId;
                        member.CampaignAmount = selectedCampaign.MemberShipFee;
                        member.AmountRecieved = 0;
                        member.PaidDate = null;
                        member.PaymentTypeId = null;
                        member.PaymentReceivedBy = null;
                        member.PaymentRemarks = null;
                    }

                    member.UpdatedBy = loggedInUser;
                    member.UpdatedDate = now;
                }


                await _dbContext.SaveChangesAsync();

                var existingCollections = await _dbContext.Campaigns
                  .Where(c => c.CollectionAdded == 1 && c.CampaignId != campaignId)
                  .ToListAsync();

                foreach (var campaign in existingCollections)
                {
                    campaign.CollectionAdded = 0;
                    campaign.UpdatedBy = loggedInUser;
                    campaign.UpdatedDate = now;
                }

                await _dbContext.SaveChangesAsync();
                selectedCampaign.Active = true;
                selectedCampaign.CollectionAdded = 1;
                selectedCampaign.UpdatedBy = loggedInUser;
                selectedCampaign.UpdatedDate = now;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task<ResponseEntity<bool>> UpdateMembershipFeeAsync(MembershipFeeViewModel model)
        {
            var response = new ResponseEntity<bool>();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var postmodel = new PostMembershipViewModel
                {
                    IssueId = model.MemberID,
                    CampaignId = model.Campaign,
                    AmountRecieved = model.PaidAmount,
                    PaymentTypeId = model.PaymentType
                };

                var feeResult = await _feeCollectionReport.FeeCollection(postmodel);

                if (feeResult.transactionStatus == HttpStatusCode.OK)
                {
                    await transaction.CommitAsync();

                    response.returnData = true;
                    response.transactionStatus = HttpStatusCode.OK;
                    response.returnMessage = "Membership fee updated successfully.";
                }
                else
                {
                    await transaction.RollbackAsync();

                    response.returnData = false;
                    response.transactionStatus = feeResult.transactionStatus;
                    response.returnMessage = feeResult.returnMessage;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = $"An error occurred: {ex.Message}";
                response.returnData = false;
            }

            return response;
        }


        public async Task<bool> ToggleCampaignStatusAsync(long campaignId, bool activate)
        {
            var campaign = await _dbContext.Campaigns.FirstOrDefaultAsync(c => c.CampaignId == campaignId);
            if (campaign == null)
                return false;

            if (activate)
            {
                var activeCount = await _dbContext.Campaigns.CountAsync(c => c.Active);
                if (activeCount >= 2)
                    throw new InvalidOperationException("Maximum of 2 campaigns can be active.");
            }

            campaign.Active = activate;
            campaign.CollectionAdded = 0;
            campaign.UpdatedBy = loggedInUser;
            campaign.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<int> GetActiveCampaignCountAsync()
        {
            return await _dbContext.Campaigns.CountAsync(c => c.Active);
        }






    }
}
