using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.DashBoard;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public DashboardRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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

        //public ResponseEntity<DashBoardData> GetMemberDataWithpaidandunpaid()
        //{
        //    var retModel = new ResponseEntity<DashBoardData>();
        //    retModel.returnData = new DashBoardData();
        //    try
        //    {
        //        var lookUpMastersData = _dbContext.LookupMasters.Where(i => i.Active);
        //        var membershipAcceptedData = _dbContext.MembershipAcceptedDatas.Where(m => m.Active == true);

        //        #region areaWiseData
        //        var areaWiseData = _dbContext.AreaDatas
        //           .Where(a => a.Active == true)
        //           .GroupJoin(
        //               membershipAcceptedData,
        //               area => area.AreaId,
        //               member => member.AreaId,
        //               (area, members) => new
        //               {
        //                   area.AreaName,
        //                   Members = members
        //               })
        //           .Select(g => new DashBoardViewModel
        //           {
        //               AreaName = g.AreaName,
        //               TotalMembers = g.Members.Count(),
        //               PaidMembers = g.Members.Count(x => (x.AmountRecieved ?? 0) != 0),
        //               UnpaidMembers = g.Members.Count(x => (x.AmountRecieved ?? 0) == 0)
        //           }).ToList();
        //        #endregion areaWiseData

        //        #region  ZoneWiseData
        //        var ZoneWiseData = _dbContext.Zones
        //        .Where(a => a.Active == true)
        //        .GroupJoin(
        //            membershipAcceptedData,
        //             zone => zone.ZoneId,
        //             member => member.ZoneId,
        //             (zone, members) => new
        //             {
        //                 zone.ZoneName,
        //                 PaidCount = members.Count(m => (m.AmountRecieved ?? 0) != 0),
        //                 UnpaidCount = members.Count(m => (m.AmountRecieved ?? 0) == 0 || m.AmountRecieved == null)
        //             })
        //        .Select(g => new SunburstNode
        //        {
        //            name = g.ZoneName,
        //            children = new List<SunburstNode>
        //             {
        //                 new SunburstNode { name = "Paid", value = g.PaidCount },
        //                 new SunburstNode { name = "Unpaid", value = g.UnpaidCount }
        //             }
        //        }).ToList();
        //        #endregion ZoneWiseData 

        //        #region UnitwiseData
        //        var UnitWiseData = _dbContext.Units
        //        .Where(a => a.Active == true)
        //        .GroupJoin(
        //            membershipAcceptedData,
        //             unit => unit.UnitId,
        //             member => member.UnitId,
        //             (unit, members) => new
        //             {
        //                 unit.UnitName,
        //                 PaidCount = members.Count(m => (m.AmountRecieved ?? 0) != 0),
        //                 UnpaidCount = members.Count(m => (m.AmountRecieved ?? 0) == 0 || m.AmountRecieved == null)
        //             })
        //        .Select(g => new SunburstNode
        //        {
        //            name = g.UnitName,
        //            children = new List<SunburstNode>
        //             {
        //                                         new SunburstNode { name = "Paid", value = g.PaidCount },
        //                                         new SunburstNode { name = "Unpaid", value = g.UnpaidCount }
        //             }
        //        }).ToList();
        //        #endregion UnitWiseData

        //        #region DistricwiseData
        //        var DistrictWiseData = lookUpMastersData.Where(i => i.LookUpTypeId == 2 && i.Active).GroupJoin(
        //            membershipAcceptedData,
        //            district => district.LookUpId,
        //            member => member.DistrictId,
        //            (district, members) => new
        //            {
        //                district.LookUpName,
        //                Members = members
        //            })
        //        .Select(g => new DistrictGenderData
        //        {
        //            DistrictName = g.LookUpName,
        //            TotalCount = g.Members.Count(),
        //            MaleCount = g.Members.Where(i => i.GenderId == 9).Count(),
        //            FemaleCount = g.Members.Where(i => i.GenderId == 10).Count()
        //        }).OrderByDescending(i=>i.TotalCount).ToList();
        //        #endregion DistricwiseData

        //        #region BloodGroupData
        //        var BloodGroupList = lookUpMastersData.
        //        Where(i => i.LookUpTypeId == 5 && i.Active).GroupJoin(
        //           membershipAcceptedData,
        //            bloodGroup => bloodGroup.LookUpId,
        //            member => member.BloodGroupId,
        //            (bloodGroup, members) => new
        //            {
        //                bloodGroup.LookUpName,
        //                MemberCount = members.Count()
        //            })
        //        .Select(g => new MemberData
        //        {
        //            Name = g.LookUpName,
        //            Count = g.MemberCount
        //        }).ToList();
        //        #endregion BloodGroupData

        //        #region DepartmentData
        //        var DepartMentList = _dbContext.Departments.
        //        Where(i => i.Active).GroupJoin(
        //            membershipAcceptedData,
        //            department => department.DepartmentId,
        //            member => member.DepartmentId,
        //            (department, members) => new
        //            {
        //                department.DepartmentName,
        //                Members = members
        //            })
        //        .Select(g => new DepartmentGenderData
        //        {
        //           DepartmentName = g.DepartmentName,
        //           TotalCount = g.Members.Count(),
        //           MaleCount = g.Members.Where(i => i.GenderId == 9).Count(),
        //           FemaleCount = g.Members.Where(i => i.GenderId == 10).Count()
        //        }).ToList();
        //        #endregion DepartmentData

        //        #region GenderData
        //        var GenderData = lookUpMastersData.Where(i => i.LookUpTypeId == 4 && i.Active).GroupJoin(
        //            membershipAcceptedData,
        //            Gender => Gender.LookUpId,
        //            member => member.GenderId,
        //            (gender, members) => new
        //            {
        //                gender.LookUpName,
        //                MemberCount = members.Count()
        //            })
        //        .Select(g => new MemberData
        //        {
        //           Name = g.LookUpName,
        //           Count = g.MemberCount
        //        }).ToList();
        //        #endregion GenderData

        //        #region WorkPlaceData
        //        var WorkPlaceData = _dbContext.WorkPlace.Where(i => i.Active).GroupJoin(
        //            membershipAcceptedData,
        //            WorkPlace => WorkPlace.WorkPlaceId,
        //            member => member.WorkPlaceId,
        //            (WorkPlace, members) => new
        //            {
        //                WorkPlace.WorkPlaceName,
        //                MemberCount = members.Count()
        //            })
        //        .Select(g => new MemberData
        //        {
        //            Name = g.WorkPlaceName,
        //            Count = g.MemberCount
        //        }).ToList();
        //        #endregion WorkPlaceData

        //        #region ProffessionData
        //        var ProffesionData = _dbContext.Professions.Where(i => i.Active).GroupJoin(
        //           membershipAcceptedData,
        //           Profession => Profession.ProfessionId,
        //           member => member.ProfessionId,
        //           (profession, members) => new
        //           {
        //               profession.ProffessionName,
        //               MemberCount = members.Count()
        //           })
        //        .Select(g => new MemberData
        //        {
        //            Name = g.ProffessionName,
        //            Count = g.MemberCount
        //        }).ToList();
        //        #endregion ProffessionData

        //        #region firstbarGraph
        //        var totalCount = membershipAcceptedData.Count();
        //        var activeCount = membershipAcceptedData.Count(i => i.AmountRecieved != 0);
        //        var campaignId = _dbContext.Campaigns.FirstOrDefault(i => i.Active)?.CampaignId;
        //        var newCount = membershipAcceptedData.Count(i => i.CampaignId == campaignId);

        //        // If "recent" means same as total, we can reuse totalCount
        //        var AllmemberData = new List<MemberData>
        //        {
        //            new MemberData { Name = "Total Members", Count = totalCount },
        //            new MemberData { Name = "Active Members", Count = activeCount },
        //            new MemberData { Name = "New Members", Count = newCount },
        //            new MemberData { Name = "Recent Members", Count = totalCount }
        //        };
        //        #endregion firstbarGraph

        //        #region AgeWiseGraph
        //        var AgeWiseGenderData = membershipAcceptedData
        //            .Where(m => m.DateofBirth != null && m.GenderId != null)
        //            .Select(m => new
        //            {
        //                GenderId = m.GenderId,
        //                Age = (int)(DateTime.Now.Year - m.DateofBirth.Value.Year)
        //            })
        //            .Where(m => m.Age >= 20)
        //            .AsEnumerable()
        //            .GroupBy(m =>
        //            {
        //                int rangeStart = 20 + ((m.Age - 20) / 10) * 10;
        //                int rangeEnd = rangeStart + 9;
        //                return $"{rangeStart}-{rangeEnd}";
        //            })
        //            .Select(g => new SunburstNode
        //            {
        //                name = g.Key, // "20-29", "30-39", etc.
        //                children = new List<SunburstNode>
        //                {
        //                    new SunburstNode { name = "Male", value = g.Count(x => x.GenderId == 9) },
        //                    new SunburstNode { name = "Female", value = g.Count(x => x.GenderId == 10) }
        //                }
        //            })
        //            .ToList();
        //        #endregion AgeWiseGraph

        //        #region ExpWiseGraph
        //        var ExpWiseGenderData = membershipAcceptedData
        //            .Where(m => m.WorkYear != null && m.GenderId != null)
        //            .Select(m => new
        //            {
        //                GenderId = m.GenderId,
        //                Exp = (int)(DateTime.Now.Year - m.WorkYear)
        //            })
        //            .Where(m => m.Exp >= 0)
        //            .AsEnumerable()
        //            .GroupBy(m =>
        //            {
        //                int rangeStart = 1 + ((m.Exp - 1) / 5) * 5;
        //                int rangeEnd = rangeStart + 4;
        //                return $"{rangeStart}-{rangeEnd}";
        //            })
        //            .Select(g => new SunburstNode
        //            {
        //                name = g.Key, // "20-29", "30-39", etc.
        //                children = new List<SunburstNode>
        //                {
        //                    new SunburstNode { name = "Male", value = g.Count(x => x.GenderId == 9) },
        //                    new SunburstNode { name = "Female", value = g.Count(x => x.GenderId == 10) }
        //                }
        //            })
        //            .ToList();
        //        #endregion ExpWiseGraph

        //        retModel.returnData.BloodGroupData = BloodGroupList;
        //        retModel.returnData.UnitData = UnitWiseData;
        //        retModel.returnData.ZoneData = ZoneWiseData;
        //        retModel.returnData.AreaData = areaWiseData.OrderByDescending(i => i.TotalMembers).ToList();
        //        retModel.returnData.DistrictData = DistrictWiseData;
        //        retModel.returnData.DepartmentData = DepartMentList.OrderByDescending(i => i.TotalCount).ToList();
        //        retModel.returnData.GenderData = GenderData;
        //        retModel.returnData.WorkPlaceData = WorkPlaceData.OrderByDescending(i => i.Count).ToList();
        //        retModel.returnData.ProffesionData = ProffesionData.OrderByDescending(i => i.Count).ToList();
        //        retModel.returnData.OrgMemberData = AllmemberData;
        //        retModel.returnData.AgewiseCategory = AgeWiseGenderData;
        //        retModel.returnData.ExpwiseCategory = ExpWiseGenderData;
        //        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
        //        retModel.returnMessage = ex.Message; // Include error message for debugging
        //    }
        //    return retModel;
        //}

        public ResponseEntity<DashBoardData> GetMemberDataWithpaidandunpaid()
        {
            var retModel = new ResponseEntity<DashBoardData>();
            retModel.returnData = new DashBoardData();
            try
            {
                // Single query to get all required lookup data with filtering
                var lookUpMastersDict = _dbContext.LookupMasters
                    .Where(i => i.Active && (i.LookUpTypeId == 2 || i.LookUpTypeId == 4 || i.LookUpTypeId == 5))
                    .ToLookup(x => x.LookUpTypeId);

                // Single query to get all active membership data with required joins
                var membershipData = _dbContext.MembershipAcceptedDatas
                    .Where(m => m.Active == true)
                    .Select(m => new
                    {
                        m.AreaId,
                        m.ZoneId,
                        m.UnitId,
                        m.DistrictId,
                        m.GenderId,
                        m.BloodGroupId,
                        m.DepartmentId,
                        m.WorkPlaceId,
                        m.ProfessionId,
                        m.CampaignId,
                        m.DateofBirth,
                        m.WorkYear,
                        AmountReceived = m.AmountRecieved ?? 0,
                        IsPaid = (m.AmountRecieved ?? 0) != 0
                    })
                    .ToList();

                // Get related master data in single queries
                var areas = _dbContext.AreaDatas.Where(a => a.Active).ToDictionary(a => a.AreaId, a => a.AreaName);
                var zones = _dbContext.Zones.Where(z => z.Active).ToDictionary(z => z.ZoneId, z => z.ZoneName);
                var units = _dbContext.Units.Where(u => u.Active).ToDictionary(u => u.UnitId, u => u.UnitName);
                var departments = _dbContext.Departments.Where(d => d.Active).ToDictionary(d => d.DepartmentId, d => d.DepartmentName);
                var workPlaces = _dbContext.WorkPlace.Where(w => w.Active).ToDictionary(w => w.WorkPlaceId, w => w.WorkPlaceName);
                var professions = _dbContext.Professions.Where(p => p.Active).ToDictionary(p => p.ProfessionId, p => p.ProffessionName);
                var activeCampaignId = _dbContext.Campaigns.Where(c => c.Active).Select(c => c.CampaignId).FirstOrDefault();

                var currentYear = DateTime.Now.Year;

                #region AreaWiseData
                var areaWiseData = new List<DashBoardViewModel>();


                    //areaWiseData = membershipData
                    //.Where(m => areas.ContainsKey((long)m.AreaId))
                    //.GroupBy(m => m.AreaId)
                    //.Select(g => new DashBoardViewModel
                    //{
                    //    AreaName = areas[(long)g.Key],
                    //    TotalMembers = g.Count(),
                    //    PaidMembers = g.Count(x => x.IsPaid),
                    //    UnpaidMembers = g.Count(x => !x.IsPaid)
                    //})
                    //.OrderByDescending(i => i.TotalMembers)
                    //.ToList();
                #endregion

                #region ZoneWiseData
                var zoneWiseData = new List<SunburstNode>();

                    //zoneWiseData  = membershipData
                    //.Where(m => zones.ContainsKey((long)m.ZoneId))
                    //.GroupBy(m => m.ZoneId)
                    //.Select(g => new SunburstNode
                    //{
                    //    name = zones[(long)g.Key],
                    //    children = new List<SunburstNode>
                    //    {
                    //new SunburstNode { name = "Paid", value = g.Count(m => m.IsPaid) },
                    //new SunburstNode { name = "Unpaid", value = g.Count(m => !m.IsPaid) }
                    //    }
                    //})
                    //.ToList();
                #endregion

                #region UnitWiseData
                var unitWiseData = new List<SunburstNode>();

                //unitWiseData = membershipData
                //    .Where(m => units.ContainsKey((long)m.UnitId))
                //    .GroupBy(m => m.UnitId)
                //    .Select(g => new SunburstNode
                //    {
                //        name = units[(long)g.Key],
                //        children = new List<SunburstNode>
                //        {
                //    new SunburstNode { name = "Paid", value = g.Count(m => m.IsPaid) },
                //    new SunburstNode { name = "Unpaid", value = g.Count(m => !m.IsPaid) }
                //        }
                //    })
                //    .ToList();
                #endregion

                #region DistrictWiseData
                //var districtLookup = lookUpMastersDict[2].ToDictionary(x => x.LookUpId, x => x.LookUpName);
                var districtWiseData = new List<DistrictGenderData>();

                //districtWiseData = membershipData
                //    .Where(m => districtLookup.ContainsKey((long)m.DistrictId))
                //    .GroupBy(m => m.DistrictId)
                //    .Select(g => new DistrictGenderData
                //    {
                //        DistrictName = districtLookup[(long)g.Key],
                //        TotalCount = g.Count(),
                //        MaleCount = g.Count(i => i.GenderId == 9),
                //        FemaleCount = g.Count(i => i.GenderId == 10)
                //    })
                //    .OrderByDescending(i => i.TotalCount)
                //    .ToList();
                #endregion

                #region BloodGroupData
                //var bloodGroupLookup = lookUpMastersDict[5].ToDictionary(x => x.LookUpId, x => x.LookUpName);
                var bloodGroupData = new List<MemberData>();

                //bloodGroupData = membershipData
                //    .Where(m => bloodGroupLookup.ContainsKey((long)m.BloodGroupId))
                //    .GroupBy(m => m.BloodGroupId)
                //    .Select(g => new MemberData
                //    {
                //        Name = bloodGroupLookup[(long)g.Key],
                //        Count = g.Count()
                //    })
                //    .ToList();
                #endregion

                #region DepartmentData
                var departmentData = new List<DepartmentGenderData>();

                //   departmentData =  membershipData
                //    .Where(m => departments.ContainsKey((long)m.DepartmentId))
                //    .GroupBy(m => m.DepartmentId)
                //    .Select(g => new DepartmentGenderData
                //    {
                //        DepartmentName = departments[(long)g.Key],
                //        TotalCount = g.Count(),
                //        MaleCount = g.Count(i => i.GenderId == 9),
                //        FemaleCount = g.Count(i => i.GenderId == 10)
                //    })
                //    .OrderByDescending(i => i.TotalCount)
                //    .ToList();
                #endregion

                #region GenderData
                var genderLookup = lookUpMastersDict[4].ToDictionary(x => x.LookUpId, x => x.LookUpName);
                var genderData = new List<MemberData>();

                //genderData = membershipData
                //    .Where(m => genderLookup.ContainsKey((long)m.GenderId))
                //    .GroupBy(m => m.GenderId)
                //    .Select(g => new MemberData
                //    {
                //        Name = genderLookup[(long)g.Key],
                //        Count = g.Count()
                //    })
                //    .ToList();
                #endregion

                #region WorkPlaceData
                var workPlaceData = new List<MemberData>();

                 //workPlaceData = membershipData
                 //   .Where(m => workPlaces.ContainsKey((long)m.WorkPlaceId))
                 //   .GroupBy(m => m.WorkPlaceId)
                 //   .Select(g => new MemberData
                 //   {
                 //       Name = workPlaces[(long)g.Key],
                 //       Count = g.Count()
                 //   })
                 //   .OrderByDescending(i => i.Count)
                 //   .ToList();
                #endregion

                #region ProfessionData
                var professionData = new List<MemberData>();

                //professionData = membershipData
                //    .Where(m => professions.ContainsKey((long)m.ProfessionId))
                //    .GroupBy(m => m.ProfessionId)
                //    .Select(g => new MemberData
                //    {
                //        Name = professions[(long)g.Key],
                //        Count = g.Count()
                //    })
                //    .OrderByDescending(i => i.Count)
                //    .ToList();
                #endregion

                #region FirstBarGraph
                var allMemberData = new List<MemberData>();

                var totalCount = membershipData.Count;
                var activeCount = membershipData.Count(i => i.IsPaid);
                var newCount = membershipData.Count(i => i.CampaignId == activeCampaignId);

                allMemberData = new List<MemberData>
                {
                    new MemberData { Name = "Total Members", Count = totalCount },
                    new MemberData { Name = "Active Members", Count = activeCount },
                    new MemberData { Name = "New Members", Count = newCount },
                    new MemberData { Name = "Recent Members", Count = totalCount }
                };
                #endregion

                #region AgeWiseGraph
                //var ageWiseGenderData = membershipData
                //    .Where(m => m.DateofBirth != null && m.GenderId != null)
                //    .Select(m => new { GenderId = m.GenderId, Age = currentYear - m.DateofBirth.Value.Year })
                //    .Where(m => m.Age >= 20)
                //    .GroupBy(m => $"{20 + ((m.Age - 20) / 10) * 10}-{20 + ((m.Age - 20) / 10) * 10 + 9}")
                //    .Select(g => new SunburstNode
                //    {
                //        name = g.Key,
                //        children = new List<SunburstNode>
                //        {
                //    new SunburstNode { name = "Male", value = g.Count(x => x.GenderId == 9) },
                //    new SunburstNode { name = "Female", value = g.Count(x => x.GenderId == 10) }
                //        }
                //    })
                //    .ToList();
                #endregion

                #region ExpWiseGraph
                //var expWiseGenderData = membershipData
                //    .Where(m => m.WorkYear != null && m.GenderId != null)
                //    .Select(m => new { GenderId = m.GenderId, Exp = currentYear - m.WorkYear })
                //    .Where(m => m.Exp >= 0)
                //    .GroupBy(m => $"{1 + ((m.Exp - 1) / 5) * 5}-{1 + ((m.Exp - 1) / 5) * 5 + 4}")
                //    .Select(g => new SunburstNode
                //    {
                //        name = g.Key,
                //        children = new List<SunburstNode>
                //        {
                //    new SunburstNode { name = "Male", value = g.Count(x => x.GenderId == 9) },
                //    new SunburstNode { name = "Female", value = g.Count(x => x.GenderId == 10) }
                //        }
                //    })
                //    .ToList();
                #endregion

                //Assign all data to return model
                retModel.returnData.BloodGroupData = bloodGroupData;
                retModel.returnData.UnitData = unitWiseData;
                retModel.returnData.ZoneData = zoneWiseData;
                retModel.returnData.AreaData = areaWiseData;
                retModel.returnData.DistrictData = districtWiseData;
                retModel.returnData.DepartmentData = departmentData;
                retModel.returnData.GenderData = genderData;
                retModel.returnData.WorkPlaceData = workPlaceData;
                retModel.returnData.ProffesionData = professionData;
                retModel.returnData.OrgMemberData = allMemberData;
                retModel.returnData.AgewiseCategory = new List<SunburstNode>();
                retModel.returnData.ExpwiseCategory = new List<SunburstNode>();
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }
            return retModel;
        }
        public ResponseEntity<List<MemberData>> GetMembersByAgeGroup(long type)
        {
            var retModel = new ResponseEntity<List<MemberData>>();
            retModel.returnData = new List<MemberData>();
            try
            {
                var MemberData = _dbContext.MembershipAcceptedDatas.Where(i => i.Active);
                if (MemberData != null)
                {
                    switch (type)
                    {
                        case 1:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 20 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 30);
                            break;
                        case 2:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 31 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 40);
                            break;
                        case 3:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 41 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 50);
                            break;
                        case 4:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 51 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 60);
                            break;
                        case 5:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 61 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 70);
                            break;
                        case 6:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 71 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 80);
                            break;
                        case 7:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 81 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 90);
                            break;
                        case 8:
                            MemberData = MemberData.Where(i => i.DateofBirth != null && (DateTime.Now.Year - i.DateofBirth.Value.Year) >= 91 && (DateTime.Now.Year - i.DateofBirth.Value.Year) <= 100);
                            break;
                    }

                    if (MemberData != null)
                    {
                        var GenderData = _dbContext.LookupMasters.Where(i => i.LookUpTypeId == 4 && i.Active).GroupJoin(
                              MemberData,
                              Gender => Gender.LookUpId,
                              member => member.GenderId,
                              (gender, members) => new
                              {
                                  gender.LookUpName,
                                  MemberCount = members.Count()
                              })
                        .Select(g => new MemberData
                        {
                            Name = g.LookUpName,
                            Count = g.MemberCount
                        }).ToList();
                        retModel.returnData = GenderData;

                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }
            return retModel;
        }

        public ResponseEntity<List<MemberData>> GetMembersByExperience(long type)
        {
            var retModel = new ResponseEntity<List<MemberData>>();
            retModel.returnData = new List<MemberData>();
            try
            {
                var MemberData = _dbContext.MembershipAcceptedDatas.Where(i => i.Active);
                if (MemberData != null)
                {
                    switch (type)
                    {
                        case 1:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 0 && (DateTime.Now.Year - i.WorkYear) <= 5);
                            break;
                        case 2:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 6 && (DateTime.Now.Year - i.WorkYear) <= 10);
                            break;
                        case 3:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 11 && (DateTime.Now.Year - i.WorkYear) <= 15);
                            break;
                        case 4:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 16 && (DateTime.Now.Year - i.WorkYear) <= 20);
                            break;
                        case 5:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 21 && (DateTime.Now.Year - i.WorkYear) <= 25);
                            break;
                        case 6:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 26 && (DateTime.Now.Year - i.WorkYear) <= 30);
                            break;
                        case 7:
                            MemberData = MemberData.Where(i => i.Memberfrom != null && (DateTime.Now.Year - i.WorkYear) >= 31 && (DateTime.Now.Year - i.WorkYear) <= 35);
                            break;
                    }

                    if (MemberData != null)
                    {
                        var GenderData = _dbContext.LookupMasters.Where(i => i.LookUpTypeId == 4 && i.Active).GroupJoin(
                              MemberData,
                              Gender => Gender.LookUpId,
                              member => member.GenderId,
                              (gender, members) => new
                              {
                                  gender.LookUpName,
                                  MemberCount = members.Count()
                              })
                        .Select(g => new MemberData
                        {
                            Name = g.LookUpName,
                            Count = g.MemberCount
                        }).ToList();
                        retModel.returnData = GenderData;
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }
            return retModel;
        }
    }
}
