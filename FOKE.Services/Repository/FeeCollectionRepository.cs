using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.DashBoard;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class FeeCollectionRepository : IFeeCollectionReport
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public FeeCollectionRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<int> GetPaidCountAsync(long campaignId)
        {
            //return await _dbContext.MembershipFees
            //    .Where(m => m.Campaign == campaignId && m.PaidAmount > 0)
            //    .CountAsync();
            return await _dbContext.MembershipFees
      .Where(m => m.Campaign == campaignId && m.PaidAmount > 0)
      .Select(m => m.MemberID) // Select only MemberId
      .Distinct()              // Ensure uniqueness
      .CountAsync();
        }

        public async Task<int> GetUnpaidCountAsync(long campaignId)
        {
            //return await _dbContext.MembershipFees
            //    .Where(m => m.Campaign == campaignId && m.PaidAmount == 0)
            //    .CountAsync();

            //    return await _dbContext.MembershipFees
            //.AsNoTracking()
            //.Where(m => m.Campaign == campaignId &&
            //            (!m.PaidAmount.HasValue || m.PaidAmount == 0))
            //.CountAsync();
            return await _dbContext.MembershipFees
           .AsNoTracking()
           .Where(m => m.Campaign == campaignId &&
                       (!m.PaidAmount.HasValue || m.PaidAmount == 0))
           .Select(m => m.MemberID)
           .Distinct()
           .CountAsync();

        }


        public ResponseEntity<List<DashBoardViewModel>> GetZoneCountwithpaidandunpaid(long campaignId)
        {
            var retModel = new ResponseEntity<List<DashBoardViewModel>>();
            try
            {
                var zones = _dbContext.Zones.Where(z => z.Active).ToList();
                var fees = _dbContext.MembershipFees
                    .Where(f => f.Campaign == campaignId)
                    .Select(f => new { f.MemberID, f.PaidAmount })
                    .ToList();

                var feesWithZone = from fee in fees
                                   join member in _dbContext.MembershipAcceptedDatas
                                      on fee.MemberID equals member.IssueId
                                   select new
                                   {
                                       member.ZoneId,
                                       fee.PaidAmount
                                   };

                var grouped = feesWithZone
                    .GroupBy(x => x.ZoneId)
                    .Select(g => new
                    {
                        ZoneId = g.Key,
                        PaidCount = g.Count(x => x.PaidAmount != null && x.PaidAmount != 0),
                        UnpaidCount = g.Count(x => x.PaidAmount == null || x.PaidAmount == 0),
                        TotalCount = g.Count()
                    })
                    .ToList();

                var result = zones.Select(z =>
                {
                    var group = grouped.FirstOrDefault(f => f.ZoneId == z.ZoneId);
                    return new DashBoardViewModel
                    {
                        AreaName = z.ZoneName,
                        TotalMembers = group?.TotalCount ?? 0,
                        PaidMembers = group?.PaidCount ?? 0,
                        UnpaidMembers = group?.UnpaidCount ?? 0
                    };
                })
                .OrderBy(x => x.AreaName)
                .ToList();

                retModel.returnData = result;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }


        public ResponseEntity<List<DashBoardViewModel>> GetAreaCountwithpaidandunpaid(long campaignId)
        {
            var retModel = new ResponseEntity<List<DashBoardViewModel>>();
            try
            {
                // 1. Get all areas (active)
                var areas = _dbContext.AreaDatas.Where(a => a.Active).ToList();

                // 2. Get fees for campaign with IssueId and AmountReceived
                var fees = _dbContext.MembershipFees
                    .Where(f => f.Campaign == campaignId)
                    .Select(f => new { f.MemberID, f.PaidAmount })
                    .ToList();

                // 3. Join fees with MembershipAcceptedDatas to get area id
                var feesWithArea = from fee in fees
                                   join member in _dbContext.MembershipAcceptedDatas
                                       on fee.MemberID equals member.IssueId
                                   select new
                                   {
                                       member.AreaId,
                                       AmountRecieved = fee.PaidAmount
                                   };

                // 4. Group by area and calculate counts
                var feesGroupedByArea = feesWithArea
                    .GroupBy(x => x.AreaId)
                    .Select(g => new
                    {
                        AreaId = g.Key,
                        PaidCount = g.Count(x => x.AmountRecieved != null && x.AmountRecieved != 0),
                        UnpaidCount = g.Count(x => x.AmountRecieved == null || x.AmountRecieved == 0),
                        TotalCount = g.Count()
                    })
                    .ToList();

                // 5. Map all areas, set counts to 0 if area not in feesGroupedByArea
                var result = areas.Select(a =>
                {
                    var group = feesGroupedByArea.FirstOrDefault(f => f.AreaId == a.AreaId);
                    return new DashBoardViewModel
                    {
                        AreaName = a.AreaName,
                        TotalMembers = group?.TotalCount ?? 0,
                        PaidMembers = group?.PaidCount ?? 0,
                        UnpaidMembers = group?.UnpaidCount ?? 0
                    };
                }).OrderBy(x => x.AreaName).ToList();

                retModel.returnData = result;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }
            return retModel;
        }



        public ResponseEntity<List<DashBoardViewModel>> GetUnitCountWithpaidAndunpaid(long campaignId)
        {
            var retModel = new ResponseEntity<List<DashBoardViewModel>>();
            try
            {
                var units = _dbContext.Units.Where(u => u.Active).ToList();
                var fees = _dbContext.MembershipFees
                    .Where(f => f.Campaign == campaignId)
                    .Select(f => new { f.MemberID, f.PaidAmount })
                    .ToList();

                var feesWithUnit = from fee in fees
                                   join member in _dbContext.MembershipAcceptedDatas
                                      on fee.MemberID equals member.IssueId
                                   select new
                                   {
                                       member.UnitId,
                                       fee.PaidAmount
                                   };

                var grouped = feesWithUnit
                    .GroupBy(x => x.UnitId)
                    .Select(g => new
                    {
                        UnitId = g.Key,
                        PaidCount = g.Count(x => x.PaidAmount != null && x.PaidAmount != 0),
                        UnpaidCount = g.Count(x => x.PaidAmount == null || x.PaidAmount == 0),
                        TotalCount = g.Count()
                    })
                    .ToList();

                var result = units.Select(u =>
                {
                    var group = grouped.FirstOrDefault(f => f.UnitId == u.UnitId);
                    return new DashBoardViewModel
                    {
                        AreaName = u.UnitName,
                        TotalMembers = group?.TotalCount ?? 0,
                        PaidMembers = group?.PaidCount ?? 0,
                        UnpaidMembers = group?.UnpaidCount ?? 0
                    };
                })
                .OrderBy(x => x.AreaName)
                .ToList();

                retModel.returnData = result;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        private const int PAYMENT_TYPE_LOOKUP_TYPE_ID = 1; // Payment Types category

        public async Task<ResponseEntity<List<DashBoardViewModel>>> GetPaymentTypeCountsAsync(long campaignId)
        {
            var ret = new ResponseEntity<List<DashBoardViewModel>>();
            try
            {
                // Group only paid fees by PaymentType (LookupMasters.LookUpId)
                var paidGrouped = _dbContext.MembershipFees
                    .Where(f => f.Campaign == campaignId
                             && (f.PaidAmount != null)
                             && f.PaidAmount != 0)
                    .GroupBy(f => f.PaymentType)
                    .Select(g => new
                    {
                        PaymentTypeId = g.Key,
                        Count = g.Count()
                    });

                // Join with lookup table to get human-readable names
                var query = paidGrouped
                    .Join(
                        _dbContext.LookupMasters
                            .Where(l => l.LookUpTypeId == PAYMENT_TYPE_LOOKUP_TYPE_ID && l.Active),
                        feeGroup => feeGroup.PaymentTypeId,
                        lookup => lookup.LookUpId,
                        (feeGroup, lookup) => new DashBoardViewModel
                        {
                            AreaName = lookup.LookUpName,
                            PaidMembers = feeGroup.Count,
                            UnpaidMembers = null,   // only focusing on paid count
                            TotalMembers = feeGroup.Count
                        }
                    )
                    .OrderBy(vm => vm.AreaName);

                ret.returnData = await query.ToListAsync();
                ret.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ret.returnData = null;
                ret.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                ret.returnMessage = ex.Message;
            }

            return ret;
        }

        public async Task<ResponseEntity<PostMembershipViewModel>> FeeCollection(PostMembershipViewModel model)
        {
            var ret = new ResponseEntity<PostMembershipViewModel>();
            try
            {
                var existingMember = await _dbContext.MembershipAcceptedDatas
                                .FirstOrDefaultAsync(c => c.IssueId == model.IssueId);
                var campaignAmt = await _dbContext.MembershipAcceptedDatas
                                    .Where(c => c.IssueId == model.IssueId)
                                    .Select(c => c.CampaignAmount)
                                    .FirstOrDefaultAsync();
                var Membershipfee = await _dbContext.MembershipFees
                                    .Where(c => c.MemberID == model.IssueId)
                                    .FirstOrDefaultAsync();
                if (existingMember != null)
                {
                    if (campaignAmt.HasValue && model.AmountRecieved == campaignAmt.Value)
                    {
                        if (model.PaymentTypeId > 0)
                        {
                            if (loggedInUser != null)
                            {
                                existingMember.AmountRecieved = model.AmountRecieved;
                                existingMember.PaymentTypeId = model.PaymentTypeId;
                                existingMember.PaymentRemarks = model.PaymentRemarks;
                                existingMember.PaidDate = DateTime.UtcNow;
                                existingMember.PaymentReceivedBy = loggedInUser;
                                _dbContext.MembershipAcceptedDatas.Update(existingMember);
                                if (Membershipfee != null)
                                {
                                    Membershipfee.PaidAmount = model.AmountRecieved;
                                    Membershipfee.PaidDate = DateTime.UtcNow;
                                    Membershipfee.CollectionRemark = model.PaymentRemarks;
                                    Membershipfee.PaymentReceivedBy = loggedInUser;
                                    Membershipfee.PaymentType = model.PaymentTypeId;
                                    _dbContext.MembershipFees.Update(Membershipfee);
                                }
                                await _dbContext.SaveChangesAsync();
                            }

                            model.IssueId = model.IssueId;
                            ret.returnData = model;
                            ret.transactionStatus = System.Net.HttpStatusCode.OK;
                            ret.returnMessage = "Registered Successfully";
                        }
                        else
                        {
                            ret.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                            ret.returnMessage = "Select Payment Type";
                        }
                    }
                    else
                    {
                        ret.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        ret.returnMessage = "Amount must be equal to Campaign Amount";
                    }

                }

            }
            catch (Exception ex)
            {
                ret.returnData = null;
                ret.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                ret.returnMessage = ex.Message;
            }

            return ret;
        }

        public async Task<ResponseEntity<List<MemberData>>> GetPaymentCollectorsData(long campaignId)
        {
            var ret = new ResponseEntity<List<MemberData>>();
            try
            {
                var CollectedByData = _dbContext.MembershipFees.Where(f => f.Campaign == campaignId && (f.PaidAmount != null) && f.PaidAmount != 0)
                                    .GroupBy(f => f.PaymentReceivedBy)
                                    .Select(g => new
                                    {
                                        userId = g.Key,
                                        Count = g.Count()
                                    })
                                    .Join(_dbContext.Users,
                                    g => g.userId,
                                    u => u.UserId,
                                    (g, u) => new MemberData
                                    {
                                        Name = u.UserName,
                                        Count = g.Count
                                    }).ToList();

                ret.returnData = CollectedByData;
                ret.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ret.returnData = null;
                ret.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                ret.returnMessage = ex.Message;
            }

            return ret;
        }


    }
}
