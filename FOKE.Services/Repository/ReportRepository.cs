using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.MembershipData.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentRepository _attachmentRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public ReportRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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

        }

        public ResponseEntity<List<PostMembershipViewModel>> GetPaymentSummaryReport(long? UserId, string? Year = null)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {
                var query = _dbContext.MembershipFees
                             .Where(m => m.PaymentReceivedBy != null)
                             .Join(_dbContext.Campaigns,
                                 m => m.Campaign,
                                 c => c.CampaignId,
                                 (m, c) => new { m, c })
                             .Join(_dbContext.Users,
                                 mc => mc.m.PaymentReceivedBy,
                                 u => u.UserId,
                                 (mc, u) => new { mc.m, mc.c, u }); // unpack m,c,u clearly

                if (UserId.HasValue)
                {
                    query = query.Where(x => x.m.PaymentReceivedBy == UserId.Value);
                }

                if(!string.IsNullOrEmpty(Year) && int.TryParse(Year, out int yearInt))
                {
                    query = query.Where(x => x.m.PaidDate.HasValue && x.m.PaidDate.Value.Year == yearInt);
                }

                var groupedResult = query.GroupBy(x => new { x.m.PaymentReceivedBy, x.u.UserName,x.m.Campaign })
                                    .Select(g => new PostMembershipViewModel
                                    {
                                        PaymentRecievedByid = g.Key.PaymentReceivedBy,
                                        UserName = g.Key.UserName,
                                        TotalReceived = g.Sum(x => Convert.ToDecimal(x.m.PaidAmount)),
                                        NoOfMembers = g.Count(),
                                        CreatedDate = g.Max(x => x.m.CreatedDate),
                                        CampaignId = g.Key.Campaign
                                    })
                                    .OrderByDescending(x => x.CreatedDate)
                                    .ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = groupedResult;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }


        public ResponseEntity<List<PostMembershipViewModel>> GetPaymentDetailReport(long? Area, long? Unit, long? Zone, long? UserId,DateTime? FromDate,DateTime? ToDate, long? CampaignID, int? pn, int? ps, string? SearchString, string? SearchColumn)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();
            try
            {
                IQueryable<PaymentQueryResult> query; // put this before the if

                if (CampaignID == null || CampaignID == 0)
                {
                     query = from m in _dbContext.MembershipAcceptedDatas.Where(i=>(i.AmountRecieved ?? 0 ) != 0)
                                join data in _dbContext.MembershipFees on m.IssueId equals data.MemberID

                                join a in _dbContext.AreaDatas on m.AreaId equals a.AreaId into areaJoin
                                from a in areaJoin.DefaultIfEmpty()

                                join u in _dbContext.Units on m.UnitId equals u.UnitId into unitJoin
                                from u in unitJoin.DefaultIfEmpty()

                                join z in _dbContext.Zones on m.ZoneId equals z.ZoneId into zoneJoin
                                from z in zoneJoin.DefaultIfEmpty()

                                join user in _dbContext.Users on m.PaymentReceivedBy equals user.UserId into userJoin
                                from user in userJoin.DefaultIfEmpty()

                                join createdUser in _dbContext.Users on m.CreatedBy equals createdUser.UserId into creatorJoin
                                from createdUser in creatorJoin.DefaultIfEmpty()

                                join c in _dbContext.Campaigns on m.CampaignId equals c.CampaignId into campaignJoin
                                from c in campaignJoin.DefaultIfEmpty()

                                where data.Active == true && (data.PaidAmount ?? 0) != 0
                                select new PaymentQueryResult
                                {
                                    m= m,
                                    AreaName = a.AreaName,
                                    UnitName = u.UnitName,
                                    ZoneName = z.ZoneName,
                                    PaymentReceivedBy = m.PaymentReceivedBy,
                                    PaymentReceivedByUser = user.UserName,
                                    CreatedByUser = createdUser.UserName,
                                    CampaignName = c.CampaignName,
                                    PaidDate = data.PaidDate
                                };

                }
                else
                {
                     query = from data in _dbContext.MembershipFees.Where(i => i.Campaign == CampaignID)
                                join m in _dbContext.MembershipAcceptedDatas on data.MemberID equals m.IssueId

                                join a in _dbContext.AreaDatas on m.AreaId equals a.AreaId into areaJoin
                                from a in areaJoin.DefaultIfEmpty()

                                join u in _dbContext.Units on m.UnitId equals u.UnitId into unitJoin
                                from u in unitJoin.DefaultIfEmpty()

                                join z in _dbContext.Zones on m.ZoneId equals z.ZoneId into zoneJoin
                                from z in zoneJoin.DefaultIfEmpty()

                                join user in _dbContext.Users on data.PaymentReceivedBy equals user.UserId into userJoin
                                from user in userJoin.DefaultIfEmpty()

                                join createdUser in _dbContext.Users on data.CreatedBy equals createdUser.UserId into creatorJoin
                                from createdUser in creatorJoin.DefaultIfEmpty()

                                join c in _dbContext.Campaigns on data.Campaign equals c.CampaignId into campaignJoin
                                from c in campaignJoin.DefaultIfEmpty()

                                where data.Active == true && (data.PaidAmount ?? 0) != 0
                                select new PaymentQueryResult
                                {
                                    m = m,
                                    AreaName = a.AreaName,
                                    UnitName = u.UnitName,
                                    ZoneName = z.ZoneName,
                                    PaymentReceivedBy = data.PaymentReceivedBy,
                                    PaymentReceivedByUser = user.UserName,
                                    CreatedByUser = createdUser.UserName,
                                    CampaignName = c.CampaignName,
                                    PaidDate = data.PaidDate
                                };
                }

                // Apply filters
                if (Area.GetValueOrDefault() != 0)
                    query = query.Where(x => x.m.AreaId == Area);
                if (Unit.GetValueOrDefault() != 0)
                    query = query.Where(x => x.m.UnitId == Unit);
                if (Zone.GetValueOrDefault() != 0)
                    query = query.Where(x => x.m.ZoneId == Zone);
                if (UserId.GetValueOrDefault() != 0)
                    query = query.Where(x => x.PaymentReceivedBy == UserId);
                query = query.OrderByDescending(x => x.m.PaidDate);

                if (FromDate.HasValue)
                {
                    DateTime from = FromDate.Value.Date;
                    query = query.Where(c => c.PaidDate >= from);
                }

                if (ToDate.HasValue)
                {
                    DateTime to = ToDate.Value.Date.AddDays(1);
                    query = query.Where(c => c.PaidDate < to);
                }

                if (!string.IsNullOrEmpty(SearchColumn) && !string.IsNullOrEmpty(SearchString))
                {
                    var search = SearchString.ToLower().Trim();

                    if (SearchColumn.ToLower() == "all")
                    {
                        // Search across multiple fields
                        query = query.Where(x =>
                            x.m.Name.ToLower().Contains(search) ||
                            x.m.CivilId.ToLower().Contains(search) ||
                            x.m.ReferanceNo.ToLower().Contains(search) ||
                            x.PaymentReceivedByUser.ToLower().Contains(search)
                        );
                    }
                    else
                    {
                        // Search inside a specific column of m
                        string filter = $"m.{SearchColumn}.ToLower().Contains(@0)";
                        query = query.Where(filter, search);
                    }
                }

                if (ps != null)
                {
                    retModel.TotalCount = query.Count();
                    var Pagenumber = Convert.ToInt32(pn);
                    var Pagesize = Convert.ToInt32(ps);
                    query = query.Skip((Pagenumber - 1) * Pagesize).Take(Pagesize);
                }

                var objModel = query.AsEnumerable() 
                .Select(x => new PostMembershipViewModel
                {
                    ReferenceNumber = x.m.ReferanceNo,
                    CivilId = x.m.CivilId,
                    Name = x.m.Name,
                    Area = x.AreaName,
                    Unit = x.UnitName,
                    Zone = x.ZoneName,
                    PaymentRecievedBy = x.PaymentReceivedByUser,
                    Active = x.m.Active,
                    CreatedUsername = x.CreatedByUser,
                    CreatedDate = x.m.CreatedDate,
                    CollectedDate = x.PaidDate,
                    AmountRecieved = x.m.AmountRecieved,
                    PaymentRemarks= x.m.PaymentRemarks,
                    CollectedDateformat = x.PaidDate?.ToString("d-MMM-yyyy"), // ✅ Now works
                    CampaignName=x.CampaignName
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


    }
}
