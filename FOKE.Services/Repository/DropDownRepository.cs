using DocumentFormat.OpenXml.Bibliography;
using FOKE.DataAccess;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.Common;
using FOKE.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Services.Repository
{
    public class DropDownRepository : IDropDownRepository
    {
        private readonly FOKEDBContext? _dbContext;
        public DropDownRepository(FOKEDBContext context)
        {
            _dbContext = context;
        }

        public List<DropDownViewModel> GetRole()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Roles.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.RoleId,
                    name = c.RoleName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetLookupTypeList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupTypeMasters.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpTypeId,
                    name = c.LookUpTypeName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetFolder()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.FolderMasters.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.FolderId,
                    name = c.FolderName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetGender()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 4)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetBloodGroup()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 5)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetProffession()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Professions.Where(c => c.Active == true).OrderBy(c => c.ProffessionName);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.ProfessionId,
                    name = c.ProffessionName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetWorkPlace()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.WorkPlace.Where(c => c.Active == true).OrderBy(c => c.WorkPlaceName);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.WorkPlaceId,
                    name = c.WorkPlaceName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetDistrict()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 2)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetArea()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.AreaDatas.Where(c => c.Active == true).OrderBy(c => c.AreaName);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.AreaId,
                    name = c.AreaName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetUnit()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Units.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.UnitId,
                    name = c.UnitName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetZone()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Zones.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.ZoneId,
                    name = c.ZoneName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetPaymentTypes()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 1)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<CampaignDropDownList> GetCampaignList()
        {
            var retModel = new List<CampaignDropDownList>();
            try
            {
                var objModel = new List<CampaignDropDownList>();
                var retData = _dbContext.Campaigns.Where(c => c.Active == true);
                objModel = retData.Select(static c => new CampaignDropDownList()
                {
                    CampaignId = (long)c.CampaignId,
                    CampaignName = c.CampaignName,
                    MemberShipFee = c.MemberShipFee
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetHearAboutUs()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 3)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetYearList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var yearList = Enumerable.Range(0, 50)
                .Select(i => DateTime.Now.Year - i)
                .Select((year, index) => new DropDownViewModel
                {
                    keyID = year,
                    name = year.ToString()
                })
                .ToList();
                retModel = yearList;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetDepartmentList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Departments.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.DepartmentId,
                    name = c.DepartmentName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetRejectedReasonList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 6)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetCancelledReasonList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.CancelReasonDatas.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.ReasonId,
                    name = c.CancelReason
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetAreasByUser(long userId)
        {
            var retModel = new List<DropDownViewModel>();

            try
            {
                var query = from am in _dbContext.AreaMembers
                            join a in _dbContext.AreaDatas on am.AreaId equals a.AreaId
                            where am.UserMemberId == userId && a.Active == true
                            orderby a.AreaName
                            select new DropDownViewModel
                            {
                                keyID = a.AreaId,
                                name = a.AreaName
                            };

                retModel = query.Distinct().ToList(); // In case the same area is linked multiple times
            }
            catch (Exception ex)
            {
                // Optionally log ex
            }

            return retModel;
        }

        public List<DropDownViewModel> GetUnitsByUser(long userId)
        {
            var retModel = new List<DropDownViewModel>();

            try
            {
                var query = from am in _dbContext.UnitMembers
                            join a in _dbContext.Units on am.UnitId equals a.UnitId
                            where am.UserMemberId == userId && a.Active == true
                            orderby a.UnitName
                            select new DropDownViewModel
                            {
                                keyID = a.UnitId,
                                name = a.UnitName
                            };

                retModel = query.Distinct().ToList();
            }
            catch (Exception ex)
            {
                // Optionally log ex
            }

            return retModel;
        }

        public List<DropDownViewModel> GetZonesByUser(long userId)
        {
            var retModel = new List<DropDownViewModel>();

            try
            {
                var query = from am in _dbContext.ZoneMembers
                            join a in _dbContext.Zones on am.ZoneId equals a.ZoneId
                            where am.UserMemberId == userId && a.Active == true
                            orderby a.ZoneName
                            select new DropDownViewModel
                            {
                                keyID = a.ZoneId,
                                name = a.ZoneName
                            };

                retModel = query.Distinct().ToList(); // In case the same area is linked multiple times
            }
            catch (Exception ex)
            {
                // Optionally log ex
            }

            return retModel;
        }

        public List<DropDownViewModel> GetTypeList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 6)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetCommitteeList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Committees.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.CommitteeId,
                    name = c.CommitteeName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }


        public List<DropDownViewModel> GetGroupList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.CommitteeGroups.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.GroupId,
                    name = c.GroupName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetSectionTypeList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 7)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }
        public List<DropDownViewModel> GetNotificationTypeList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeName == "Notification")
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetUsers()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Users.Where(c => c.Active == true);
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.UserId,
                    name = c.UserName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }


        public List<DropDownViewModel> GetallCampaignYears()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.Campaigns.Where(c => c.Active == true);
                var DataModel = retData.Select(static c => new 
                {
                    keyID = (long)c.CampaignId,
                    StartDate = (DateTime)c.StartDate,
                    EndDate = c.EndDate,
                }).ToList();


                var YearData = retData.AsEnumerable().SelectMany(c=> new[]
                {
                    new { year = c.StartDate.HasValue ? c.StartDate.Value.Year : (int?)null, Id = c.CampaignId },
                    new { year = c.EndDate.HasValue ? c.EndDate.Value.Year : (int?)null, Id = c.CampaignId }
                }).Where( x => x.year.HasValue)
                .GroupBy( x => x.year.Value)
                .Select(g => new DropDownViewModel
                {
                    keyID  = g.Select(x => x.Id).Distinct().FirstOrDefault(),
                    name = g.Key != null ? Convert.ToString(g.Key) : null
                }).OrderBy(x => x.name).ToList();

                objModel = YearData;
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetAllCampaignList()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = _dbContext.Campaigns
                    .Where(c => !string.IsNullOrEmpty(c.CampaignName))
                    .OrderByDescending(c => c.Active)
                    .ThenBy(c => c.CampaignName)
                    .Select(c => new DropDownViewModel
                    {
                        keyID = (long)c.CampaignId,
                        name = c.CampaignName
                    })
                    .ToList();

                retModel = objModel;
            }
            catch (Exception ex)
            {

            }

            return retModel;
        }

        public List<DropDownViewModel> GetAccountTypes()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 9)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

        public List<DropDownViewModel> GetCategoryTypes()
        {
            var retModel = new List<DropDownViewModel>();
            try
            {
                var objModel = new List<DropDownViewModel>();
                var retData = _dbContext.LookupMasters
                .Join(_dbContext.LookupTypeMasters,
                      l => l.LookUpTypeId,
                      ly => ly.LookUpTypeId,
                      (l, ly) => new { l, ly })
                .Where(x => x.ly.LookUpTypeId == 10)
                .Select(x => x.l)
                .ToList();
                objModel = retData.Select(static c => new DropDownViewModel()
                {
                    keyID = (long)c.LookUpId,
                    name = c.LookUpName
                }).ToList();
                retModel = objModel;
            }
            catch (Exception ex)
            {

            }
            return retModel;
        }

    }
}
