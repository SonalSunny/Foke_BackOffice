using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.DeviceData.DTO;
using FOKE.Entity.API.DeviceData.ViewModel;
using FOKE.Entity.API.DeviceLogin.ViewModel;
using FOKE.Entity.DashBoard;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.MembershipData.DTO;
using FOKE.Entity.MembershipData.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Entity.MembershipRegistration.DTO;
using FOKE.Entity.MembershipRegistration.ViewModel;
using FOKE.Services.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace FOKE.Services.Repository
{
    public class MembershipFormRepository : IMembershipFormRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IAuthenticationServiceRepository _authenticationServiceRepository;
        private readonly IFeeCollectionReport _feeCollectionReport;

        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public MembershipFormRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachmentRepository, IAuthenticationServiceRepository authenticationServiceRepository, IFeeCollectionReport feeCollectionReport)
        {
            this._dbContext = FOKEDBContext;
            _attachmentRepository = attachmentRepository;
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
            catch (Exception ex)
            {
            }

            _authenticationServiceRepository = authenticationServiceRepository;
            _feeCollectionReport = feeCollectionReport;
        }


        #region Memebrship_Registration

        public async Task<ResponseEntity<MembershipViewModel>> RegisterMember(MembershipViewModel model)
        {
            var retModel = new ResponseEntity<MembershipViewModel>();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var MembershipAcceptedDataList = _dbContext.MembershipAcceptedDatas.Where(i => i.Active).ToList();
                var memberShipRequestList = _dbContext.MembershipRequestDetails.Where(i => i.Active).ToList();
                var AcceptedMemberExists = MembershipAcceptedDataList.Any(u => u.CivilId == model.CivilId || u.ContactNo == model.ContactNo || u.PassportNo == model.PassportNo);
                var existingRequestMember = memberShipRequestList.FirstOrDefault(u => u.CivilId == model.CivilId || u.ContactNo == model.ContactNo || u.PassportNo == model.PassportNo);

                var Member = new MembershipDetails();

                if (AcceptedMemberExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Member Already Exist";
                    return retModel;
                }
                else if (existingRequestMember != null)
                {
                    // Update the request member
                    existingRequestMember.Name = model.Name;
                    existingRequestMember.CivilId = model.CivilId;
                    existingRequestMember.PassportNo = model.PassportNo;
                    existingRequestMember.DateofBirth = model.DOB;
                    existingRequestMember.GenderId = model.Genderid;
                    existingRequestMember.BloodGroupId = model.BloodGroupid;
                    existingRequestMember.ProffessionId = model.Professionid;
                    existingRequestMember.ContactNo = model.ContactNo;
                    existingRequestMember.Email = model.Email;
                    existingRequestMember.AreaId = model.Areaid;
                    existingRequestMember.CountryCode = model.CountryCodeid;
                    existingRequestMember.ProffessionOther = model.ProffessionOther;
                    existingRequestMember.WhatsAppNo = model.WhatsAppNo;
                    existingRequestMember.WhatsAppNoCountryCodeid = model.WhatsAppNoCountryCodeid;
                    existingRequestMember.Company = model.CompanyName;
                    existingRequestMember.KuwaitAddres = model.KuwaitAddres;
                    existingRequestMember.MembershipType = model.MembershipType; //1 - Single, 2 - Family
                    existingRequestMember.PermenantAddress = model.PermenantAddress;
                    existingRequestMember.Pincode = model.Pincode;
                    existingRequestMember.EmergencyContactName = model.EmergencyContactName;
                    existingRequestMember.EmergencyContactRelation = model.EmergencyContactRelation;
                    existingRequestMember.EmergencyContactCountryCodeid = model.EmergencyContactCountryCodeid;
                    existingRequestMember.EmergencyContactNumber = model.EmergencyContactNumber;
                    existingRequestMember.EmergencyContactEmail = model.EmergencyContactEmail;
                    existingRequestMember.UpdatedDate = DateTime.UtcNow;
                    existingRequestMember.UpdatedBy = loggedInUser;
                    //existingRequestMember.WorkPlaceId = model.WorkPlaceid;
                    //existingRequestMember.DistrictId = model.Districtid;
                    //existingRequestMember.HearAboutus = model.Hearaboutusid;
                    //existingRequestMember.WorkYear = model.WorkYear;
                    //existingRequestMember.WorkplaceOther = model.WorkplaceOther;
                    //existingRequestMember.DepartmentId = model.DepartmentId;

                    await _dbContext.SaveChangesAsync();
                    Member.MembershipId = existingRequestMember.MembershipId;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Member details added successfully";
                }
                else
                {
                     Member = new MembershipDetails
                    {
                        Name = model.Name,
                        CivilId = model.CivilId,
                        PassportNo = model.PassportNo,
                        DateofBirth = model.DOB,
                        GenderId = model.Genderid,
                        BloodGroupId = model.BloodGroupid,
                        ProffessionId = model.Professionid,
                        ContactNo = model.ContactNo,
                        Email = model.Email,
                        AreaId = model.Areaid,
                        CountryCode = model.CountryCodeid,
                        WorkplaceOther = model.WorkplaceOther,
                        ProffessionOther = model.ProffessionOther,
                        WhatsAppNo = model.WhatsAppNo,
                        WhatsAppNoCountryCodeid = model.WhatsAppNoCountryCodeid,
                        KuwaitAddres = model.KuwaitAddres,
                        Company = model.CompanyName,
                        MembershipType = model.MembershipType, // Membership type 1 - Single, 2 - Family, MEmber Type - 1 - Parent 2- Dependant
                        PermenantAddress = model.PermenantAddress,
                        Pincode = model.Pincode,
                        EmergencyContactName = model.EmergencyContactName,
                        EmergencyContactRelation = model.EmergencyContactRelation,
                        EmergencyContactCountryCodeid = model.EmergencyContactCountryCodeid,
                        EmergencyContactNumber = model.EmergencyContactNumber,
                        EmergencyContactEmail = model.EmergencyContactEmail,
                        ParentId = 0,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = loggedInUser,
                        Active = true,
                        //WorkPlaceId = model.WorkPlaceid,
                        //DistrictId = model.Districtid,
                        //DepartmentId = model.DepartmentId,
                        //WorkYear = model.WorkYear,
                        //HearAboutus = model.Hearaboutusid,
                    };
                    await _dbContext.MembershipRequestDetails.AddAsync(Member);
                    await _dbContext.SaveChangesAsync();
                }

                var ErrorMessage = new List<string>();
                var AdultMembersList = new List<FamilyMembersData>();
                var MinorMembersList = new List<FamilyMembersData>();

                if (model.MembershipType == 2)
                {
                    foreach (var item in model.FamilyData)
                    {
                        var GenderId = new long();
                        var DobData = GenerateDobFromCivilId(item.CivilId);
                        if (DobData.transactionStatus != HttpStatusCode.OK)
                        {
                            ErrorMessage.Add("CivilId Given For Member " + item.Name + "is Invalid");
                        }
                        else if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId || u.PassportNo == item.PassportNo)) || (memberShipRequestList.Any(u => u.CivilId == item.CivilId  || u.PassportNo == item.PassportNo)))
                        {
                            if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId)) || (memberShipRequestList.Any(u => u.CivilId == item.CivilId)))
                            {
                                retModel.returnMessage = "Civil ID Given For the Family Member " + item.Name + " " + "Already Exists";
                            }
                            //else if ((MembershipAcceptedDataList.Any(u => u.ContactNo == item.MobileNoRelative)) || (memberShipRequestList.Any(u => u.ContactNo == item.MobileNoRelative)))
                            //{
                            //    retModel.returnMessage = "Contact No Given For Family Member " + item.Name + " " + "Already Exists";
                            //}
                            else if ((MembershipAcceptedDataList.Any(u => u.PassportNo == item.PassportNo)) || (memberShipRequestList.Any(u => u.ContactNo == item.MobileNoRelative)))
                            {
                                retModel.returnMessage = "Passport No Given For Family Member " + item.Name + " " + "Already Exists";
                            }
                            retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                            return retModel;
                        }
                        else
                        {
                            item.DateOfBirth = DobData.returnData;
                        }
                        var relationName = _dbContext.LookupMasters.FirstOrDefault(r => r.LookUpId == item.RelationType)?.LookUpName;

                        if (relationName != null)
                        {
                            GenderId = GetGenderId(relationName.ToLower()) ?? 0;
                        }

                        item.GenderId = GenderId;
                        item.ParentId = Member.MembershipId;

                        var MemberAge = DateTime.UtcNow.Year - item.DateOfBirth.Value.Year;
                        if (MemberAge > 18)
                        {
                            AdultMembersList.Add(item);
                        }
                        else if (MemberAge < 18)
                        {
                            MinorMembersList.Add(item);
                        }
                    }

                    if (AdultMembersList.Any())
                    {
                        var MembersList = AdultMembersList.Select(f => new MembershipDetails
                        {
                            Name = f.Name,
                            CivilId = f.CivilId,
                            PassportNo = f.PassportNo,
                            DateofBirth = f.DateOfBirth,
                            GenderId = f.GenderId,
                            BloodGroupId = f.BloodGroupid,
                            ProffessionId = f.Professionid,
                            CountryCode = f.CountryCodeid,
                            ContactNo = f.MobileNoRelative,
                            Email = f.EmailRelative,
                            AreaId = model.Areaid,
                            Company = f.CompanyName,
                            KuwaitAddres = model.KuwaitAddres,
                            PermenantAddress = model.PermenantAddress,
                            Pincode = model.Pincode,
                            ParentId = f.ParentId,
                            Active = true,
                            CreatedDate = DateTime.UtcNow, 
                            CreatedBy = loggedInUser,
                        }).ToList();

                        _dbContext.MembershipRequestDetails.AddRange(MembersList);
                        _dbContext.SaveChanges();
                    }
                    if (MinorMembersList.Any())
                    {
                        var MembersList = MinorMembersList.Select(f => new MinorApplicantDetails
                        {
                            Name = f.Name,
                            CivilId = f.CivilId,
                            RelationType = f.RelationType,
                            PassportNo = f.PassportNo,
                            DateofBirth = f.DateOfBirth,
                            GenderId = f.GenderId,
                            BloodGroupId = f.BloodGroupid,
                            ProffessionId = f.Professionid,
                            CountryCode = model.CountryCodeid,
                            ContactNo = model.ContactNo,
                            Email = model.Email,
                            AreaId = model.Areaid,
                            KuwaitAddres = model.KuwaitAddres,
                            PermenantAddress = model.PermenantAddress,
                            Pincode = model.Pincode,
                            ParentId = f.ParentId,
                            Active = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser,
                        }).ToList();

                        _dbContext.MinorApplicantDetails.AddRange(MembersList);
                        _dbContext.SaveChanges();
                    }
                }

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Registered Successfully";
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;    
        }

        public async Task<ResponseEntity<PostMembershipViewModel>> UpdateRegisterdMember(PostMembershipViewModel model)
        {
            var retModel = new ResponseEntity<PostMembershipViewModel>();
            try
            {
                var memberData = _dbContext.MembershipRequestDetails.FirstOrDefault(i => i.MembershipId == model.MembershipId);
                var MemberExists = _dbContext.MembershipRequestDetails.Any(u => u.MembershipId != model.MembershipId && u.CivilId == model.CivilId || u.MembershipId != model.MembershipId && u.ContactNo == model.ContactNo);
                if (MemberExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Member Already Exist";
                }
                else if (memberData != null)
                {
                    memberData.Name = model.Name;
                    memberData.CivilId = model.CivilId;
                    memberData.PassportNo = model.PassportNo;
                    memberData.DateofBirth = model.DateofBirth;
                    memberData.GenderId = model.GenderId;
                    memberData.BloodGroupId = model.BloodGroupId;
                    memberData.CountryCode = model.CountryCodeId;
                    memberData.ContactNo = model.ContactNo;
                    memberData.WhatsAppNoCountryCodeid = model.WhatsAppNoCountryCodeid;
                    memberData.WhatsAppNo = model.WhatsAppNo;   
                    memberData.Email = model.Email;
                    memberData.AreaId = model.AreaId; 
                    memberData.ProffessionId = model.ProfessionId;
                    memberData.Company = model.Company; 
                    memberData.KuwaitAddres = model.KuwaitAddress;

                    memberData.MembershipType = model.MembershipType;

                    memberData.PermenantAddress = model.PermenantAddress;
                    memberData.Pincode = model.Pincode;

                    memberData.EmergencyContactName = model.EmergencyContactName;
                    memberData.EmergencyContactRelation = model.EmergencyContactRelation;
                    memberData.EmergencyContactCountryCodeid = model.EmergencyContactCountryCodeid;
                    memberData.EmergencyContactNumber = model.EmergencyContactNumber;
                    memberData.EmergencyContactEmail = model.EmergencyContactEmail;

                    memberData.UpdatedDate = DateTime.UtcNow;
                    memberData.UpdatedBy = loggedInUser;


                    if(model.FamilyData != null && model.FamilyData.Any())
                    {
                        var ErrorMessage = new List<string>();
                        var AdultMembersList = new List<FamilyMembersData>();
                        var MinorMembersList = new List<FamilyMembersData>();
                        var MembershipAcceptedDataList = _dbContext.MembershipAcceptedDatas.Where(i => i.Active).ToList();
                        var memberShipRequestList = _dbContext.MembershipRequestDetails.Where(i => i.Active).ToList();

                        var ExistingMembers = _dbContext.MinorApplicantDetails.Where(i => i.Active && i.ParentId == model.MembershipId).ToList();

                        var AddedmembersId = model.FamilyData.Select(i => i.MembershipId).ToList();

                        var ExistingMembers3 =  ExistingMembers.Where(i => AddedmembersId.Contains(i.MembershipId)).ToList();

                        foreach (var item in model.FamilyData)
                        {
                            if(item.IsChanged || item.IsNew)
                            {
                                var GenderId = new long();
                                var DobData = GenerateDobFromCivilId(item.CivilId);
                                if (DobData.transactionStatus != HttpStatusCode.OK)
                                {
                                    ErrorMessage.Add("CivilId Given For Member " + item.Name + "is Invalid");
                                }
                                else if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId ||  u.PassportNo == item.PassportNo)) || (memberShipRequestList.Any(u => u.CivilId == item.CivilId || u.PassportNo == item.PassportNo)))
                                {
                                    if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId)) || (memberShipRequestList.Any(u => u.CivilId == item.CivilId)))
                                    {
                                        retModel.returnMessage = "Civil ID Given For the Family Member " + item.Name + " " + "Already Exists";
                                    }
                                    //else if ((MembershipAcceptedDataList.Any(u => u.ContactNo == item.MobileNoRelative)) || (memberShipRequestList.Any(u => u.ContactNo == item.MobileNoRelative)))
                                    //{
                                    //    retModel.returnMessage = "Contact No Given For Family Member " + item.Name + " " + "Already Exists";
                                    //}
                                    else if ((MembershipAcceptedDataList.Any(u => u.PassportNo == item.PassportNo)) || (memberShipRequestList.Any(u => u.ContactNo == item.MobileNoRelative)))
                                    {
                                        retModel.returnMessage = "Passport No Given For Family Member " + item.Name + " " + "Already Exists";
                                    }
                                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                                    return retModel;
                                }
                                else
                                {
                                    item.DateOfBirth = DobData.returnData;
                                }
                                var relationName = _dbContext.LookupMasters.FirstOrDefault(r => r.LookUpId == item.RelationType)?.LookUpName;

                                if (relationName != null)
                                {
                                    GenderId = GetGenderId(relationName.ToLower()) ?? 0;
                                }

                                item.GenderId = GenderId;
                                item.ParentId = memberData.MembershipId;

                                var MemberAge = DateTime.UtcNow.Year - item.DateOfBirth.Value.Year;

                                if (item.IsChanged && item.IsNew)
                                {
                                    if (MemberAge > 18)
                                    {
                                        AdultMembersList.Add(item);
                                    }
                                    else if (MemberAge < 18)
                                    {
                                        MinorMembersList.Add(item);
                                    }
                                }
                                else if (item.IsChanged && !item.IsNew)
                                {
                                    var ExistingMember = ExistingMembers3.FirstOrDefault(i => i.MembershipId == item.MembershipId);
                                    if (ExistingMember != null)
                                    {
                                        ExistingMember.Name = item.Name;
                                        ExistingMember.CivilId = item.CivilId;
                                        ExistingMember.PassportNo = item.PassportNo;
                                        ExistingMember.DateofBirth = item.DateOfBirth;
                                        ExistingMember.RelationType = item.RelationType;
                                        ExistingMember.GenderId = item.GenderId;
                                        ExistingMember.BloodGroupId = item.BloodGroupid;
                                        ExistingMember.CountryCode = model.CountryCodeId;
                                        ExistingMember.ContactNo = model.ContactNo;
                                        ExistingMember.Email = model.Email;
                                        ExistingMember.AreaId = model.AreaId;
                                        ExistingMember.KuwaitAddres = model.KuwaitAddress;
                                        ExistingMember.PermenantAddress = model.PermenantAddress;
                                        ExistingMember.Pincode = model.Pincode;
                                        ExistingMember.ParentId = item.ParentId;
                                        ExistingMember.Active = true;
                                        ExistingMember.UpdatedDate = DateTime.UtcNow;
                                        ExistingMember.UpdatedBy = loggedInUser;
                                    }
                                    _dbContext.MinorApplicantDetails.Update(ExistingMember);
                                    _dbContext.SaveChanges();
                                }
                            }

                        }

                        if (AdultMembersList.Any())
                        {
                            var MembersList = AdultMembersList.Select(f => new MembershipDetails
                            {
                                Name = f.Name,
                                CivilId = f.CivilId,
                                PassportNo = f.PassportNo,
                                DateofBirth = f.DateOfBirth,
                                GenderId = f.GenderId,
                                BloodGroupId = f.BloodGroupid,
                                ProffessionId = f.Professionid,
                                CountryCode = f.CountryCodeid,
                                ContactNo = f.MobileNoRelative,
                                Email = f.EmailRelative,
                                AreaId = model.AreaId,
                                Company = f.CompanyName,
                                KuwaitAddres = model.KuwaitAddress,
                                PermenantAddress = model.PermenantAddress,
                                Pincode = model.Pincode,
                                ParentId = f.ParentId,
                                Active = true,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = loggedInUser,
                            }).ToList();

                            _dbContext.MembershipRequestDetails.AddRange(MembersList);
                            _dbContext.SaveChanges();
                        }
                        if (MinorMembersList.Any())
                        {
                            var MembersList = MinorMembersList.Select(f => new MinorApplicantDetails
                            {
                                Name = f.Name,
                                CivilId = f.CivilId,
                                RelationType = f.RelationType,
                                PassportNo = f.PassportNo,
                                DateofBirth = f.DateOfBirth,
                                GenderId = f.GenderId,
                                BloodGroupId = f.BloodGroupid,
                                CountryCode = model.CountryCodeId,
                                ContactNo = model.ContactNo,
                                Email = model.Email,
                                AreaId = model.AreaId,
                                KuwaitAddres = model.KuwaitAddress,
                                PermenantAddress = model.PermenantAddress,
                                Pincode = model.Pincode,
                                ParentId = f.ParentId,
                                Active = true,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = loggedInUser,
                            }).ToList();

                            _dbContext.MinorApplicantDetails.AddRange(MembersList);
                            _dbContext.SaveChanges();
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                    retModel.returnData = model;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public ResponseEntity<List<MembershipViewModel>> GetAllMembers(long? Areaid, long? Proffesionid, long? workplaceId)
        {
            var retModel = new ResponseEntity<List<MembershipViewModel>>();
            try
            {
                var objModel = new List<MembershipViewModel>();

                IQueryable<MembershipDetails> retData = _dbContext.MembershipRequestDetails.Where(c => c.Active == true);

                if (Areaid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.AreaId == Areaid);
                }
                if (Proffesionid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.ProffessionId == Proffesionid);
                }
                if (workplaceId.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.WorkPlaceId == workplaceId);
                }

                objModel = retData.Select(c => new MembershipViewModel()
                {
                    MembershipId = c.MembershipId,
                    Name = c.Name,
                    CivilId = c.CivilId,
                    PassportNo = c.PassportNo,
                    DOB = c.DateofBirth,
                    Genderid = c.GenderId,
                    Gender = _dbContext.LookupMasters.Where(r => r.LookUpId == c.GenderId).Select(r => r.LookUpName).FirstOrDefault(),
                    BloodGroupid = c.BloodGroupId,
                    BloodGroup = _dbContext.LookupMasters.Where(e => e.LookUpId == c.BloodGroupId).Select(e => e.LookUpName).FirstOrDefault(),
                    Professionid = c.ProffessionId,
                    Proffession = _dbContext.Professions.Where(e => e.ProfessionId == c.ProffessionId)
                        .Select(e => e.ProffessionName)
                        .FirstOrDefault(),
                    WorkPlaceid = c.WorkPlaceId,
                    WorkPlace = _dbContext.WorkPlace
                        .Where(e => e.WorkPlaceId == c.WorkPlaceId)
                        .Select(e => e.WorkPlaceName)
                        .FirstOrDefault(),
                    PhoneNo = "+" + c.CountryCode + c.ContactNo,
                    Email = c.Email,
                    District = _dbContext.LookupMasters
                        .Where(e => e.LookUpId == c.DistrictId)
                        .Select(e => e.LookUpName)
                        .FirstOrDefault(),
                    Area = _dbContext.AreaDatas
                        .Where(e => e.AreaId == c.AreaId)
                        .Select(e => e.AreaName)
                        .FirstOrDefault(),
                    CreatedDate = c.CreatedDate,
                    Active = c.Active
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

        public ResponseEntity<PostMembershipViewModel> GetMemberById(long? id)
        {
            var retModel = new ResponseEntity<PostMembershipViewModel>();
            try
            {
                var objModel = new PostMembershipViewModel();
                var MinorApplicantList = new List<FamilyMembersData>();
                var retData = _dbContext.MembershipRequestDetails.FirstOrDefault(c => c.MembershipId == id);
                objModel = new PostMembershipViewModel()
                {
                    MembershipId = retData.MembershipId,
                    Name = retData.Name,
                    CivilId = retData.CivilId,
                    PassportNo = retData.PassportNo,
                    DateofBirth = retData.DateofBirth,
                    DateofBirthString = retData.DateofBirth != null ? retData.DateofBirth.Value.Date.ToString("dd-MM-yyyy") : null,
                    GenderId = retData.GenderId,
                    Gender = _dbContext.LookupMasters .Where(r => r.LookUpId == retData.GenderId).Select(r => r.LookUpName).FirstOrDefault(),
                    BloodGroupId = retData.BloodGroupId,
                    BloodGroup = _dbContext.LookupMasters .Where(r => r.LookUpId == retData.BloodGroupId).Select(r => r.LookUpName).FirstOrDefault(),
                    ProfessionId = retData.ProffessionId,
                    Profession = _dbContext.Professions.Where(r => r.ProfessionId == retData.ProffessionId).Select(r => r.ProffessionName).FirstOrDefault(),
                    CountryCodeId = retData.CountryCode,
                    ContactNo = retData.ContactNo,
                    PhoneNo = $"+{retData.CountryCode} {retData.ContactNo}",
                    WhatsAppNoCountryCodeid = retData.WhatsAppNoCountryCodeid,
                    WhatsAppNo = retData.WhatsAppNo,
                    WhatsappNoString = $"+{retData.CountryCode} {retData.ContactNo}",
                    Email = retData.Email,
                    Company = retData.Company,
                    KuwaitAddress = retData.KuwaitAddres,
                    PermenantAddress = retData.PermenantAddress,
                    Pincode = retData.Pincode,
                    EmergencyContactName = retData.EmergencyContactName,
                    EmergencyContactRelation = retData.EmergencyContactRelation,
                    EmergencyContactRelationString = _dbContext.LookupMasters.Any(r => r.LookUpId == retData.EmergencyContactRelation) ? _dbContext.LookupMasters.Where(r => r.LookUpId == retData.EmergencyContactRelation).Select(r => r.LookUpName).FirstOrDefault() : null,
                    EmergencyContactEmail = retData.EmergencyContactEmail,
                    EmergencyContactCountryCodeid = retData.EmergencyContactCountryCodeid,
                    EmergencyContactNumber = retData.EmergencyContactNumber,
                    EmergencyContactNumberString  = $"+{retData.CountryCode} {retData.ContactNo}",
                    ProffessionOther = retData.ProffessionOther,
                    AreaId = retData.AreaId != null ? (long)retData.AreaId : 0,
                    Area = retData.AreaId != null ? _dbContext.AreaDatas.Where(e => e.AreaId == retData.AreaId).Select(e => e.AreaName).FirstOrDefault() : null,
                    CreatedDate = retData.CreatedDate,
                    //HearAboutUsId = retData.HearAboutus,
                    //DepartmentId = retData.DepartmentId,
                    //DepartmentName = retData.DepartmentId != null ? _dbContext.Departments.FirstOrDefault(i => i.DepartmentId == retData.DepartmentId).DepartmentName : "",
                    //HearAboutUs = retData.HearAboutus != null ? _dbContext.LookupMasters.Where(r => r.LookUpId == retData.HearAboutus).Select(r => r.LookUpName).FirstOrDefault() : null,
                    //WorkYear = retData.WorkYear,
                    //DistrictId = retData.DistrictId != null ? (long)retData.DistrictId : 0,
                    //District = retData.DistrictId != null ? _dbContext.LookupMasters.Where(r => r.LookUpId == retData.DistrictId).Select(r => r.LookUpName).FirstOrDefault() : null,
                    //WorkplaceOther = retData.WorkplaceOther,
                    //WorkPlaceId = retData.WorkPlaceId,
                    //WorkPlace = _dbContext.WorkPlace.Where(r => r.WorkPlaceId == retData.WorkPlaceId).Select(r => r.WorkPlaceName).FirstOrDefault(),
                };
                var MinorRelativeData = _dbContext.MinorApplicantDetails.Where(i => i.Active && i.ParentId == objModel.MembershipId).ToList();
                if(MinorRelativeData.Any())
                {
                    MinorApplicantList = MinorRelativeData.Select(
                    i => new FamilyMembersData
                    {
                        MembershipId = i.MembershipId,
                        Name = i.Name,
                        RelationType = i.RelationType,
                        RelationTypeName = i.RelationType != null ? _dbContext.LookupMasters.SingleOrDefault(r => r.LookUpId == i.RelationType).LookUpName : null,
                        CivilId = i.CivilId,
                        PassportNo = i.PassportNo,
                        DateOfBirth = i.DateofBirth,
                        DateofBirthString = i.DateofBirth != null ? i.DateofBirth.Value.Date.ToString("dd-MM-yyyy") : null,
                        GenderId = i.GenderId,
                        GenderString = i.GenderId != null && i.GenderId != 0 ? _dbContext.LookupMasters.SingleOrDefault(r => r.LookUpId == i.GenderId).LookUpName : null,
                        BloodGroupid = i.BloodGroupId,
                        BloodGroup = i.BloodGroupId != null ? _dbContext.LookupMasters.SingleOrDefault(r => r.LookUpId == i.BloodGroupId).LookUpName : null,
                        CountryCodeid = i.CountryCode,
                        MobileNoRelative = i.ContactNo,
                        MobileNoRelativeString = $"+{i.CountryCode} {i.ContactNo}",
                        EmailRelative = i.Email,
                    }).ToList();
                    objModel.FamilyData = MinorApplicantList;
                }
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
        #endregion Memebrship_Registration


        #region Issue_Memebrship
        public async Task<ResponseEntity<PostMembershipViewModel>> IssueMember(PostMembershipViewModel model)
        {
            var retModel = new ResponseEntity<PostMembershipViewModel>();
            try
            {
                if (model.MembershipStatus == 1)
                {
                    if (model.IssueId == null)
                    {
                        var IssuedMemberExists = _dbContext.MembershipAcceptedDatas
                            .Any(u => u.CivilId == model.CivilId || u.ContactNo == model.ContactNo);

                        if (IssuedMemberExists)
                        {
                            retModel.transactionStatus = HttpStatusCode.BadRequest;
                            retModel.returnMessage = "Member Already Exists With Either Same CivilId Or PhoneNo";
                            return retModel;

                        }
                        if (model.AreaId == 0 || model.ProfessionId == 0)
                        {
                            retModel.transactionStatus = HttpStatusCode.BadRequest;
                            retModel.returnMessage = "Required details are missing. Kindly provide all the necessary information.";
                            return retModel;
                        }
                        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                        try
                        {
                            var Member = new MembershipAccepted
                            {
                                ReferanceNo = GenerateReferanceNumber(true),
                                Name = model.Name,
                                CivilId = model.CivilId,
                                PassportNo = model.PassportNo,
                                DateofBirth = model.DateofBirth,
                                GenderId = model.GenderId,
                                BloodGroupId = model.BloodGroupId,
                                CountryCodeId = model.CountryCodeId,
                                ContactNo = model.ContactNo,
                                WhatsAppNoCountryCodeid = model.WhatsAppNoCountryCodeid,
                                WhatsAppNo = model.WhatsAppNo,
                                Email = model.Email,
                                ProfessionId = model.ProfessionId,
                                ProffessionOther = model.ProffessionOther,
                                Company = model.Company,
                                AreaId = model.AreaId,
                                KuwaitAddres = model.KuwaitAddress,

                                PermenantAddress = model.PermenantAddress,
                                Pincode = model.Pincode,

                                EmergencyContactName = model.EmergencyContactName,
                                EmergencyContactRelation = model.EmergencyContactRelation,
                                EmergencyContactCountryCodeid = model.EmergencyContactCountryCodeid,
                                EmergencyContactNumber = model.EmergencyContactNumber,
                                EmergencyContactEmail = model.EmergencyContactEmail,

                                ZoneId = model.ZoneId,
                                UnitId = model.UnitId,
                                ReferredBy = model.ReferredBy,
                                MembershipNo = model.MembershipNo,

                                CampaignId = model.CampaignId,
                                CampaignAmount = _dbContext.Campaigns.Where(c => c.CampaignId == model.CampaignId).Select(c => c.MemberShipFee).FirstOrDefault(),
                                MembershipRequestedDate = model.MembershipRequestedDate,
                                Memberfrom = DateTime.UtcNow,
                                ApprovedBy = loggedInUser,
                                Active = true

                                //WorkplaceOther = model.WorkplaceOther,
                                //DistrictId = model.DistrictId,
                                //ReferredBy = model.ReferredBy,
                                //WorkYear = model.WorkYear,
                                //ZoneId = model.ZoneId,
                                //UnitId = model.UnitId,
                                //WorkPlaceId = model.WorkPlaceId,
                                //DepartmentId = model.DepartmentId,
                            };
                            await _dbContext.MembershipAcceptedDatas.AddAsync(Member);
                            await _dbContext.SaveChangesAsync();
                            model.IssueId = Member.IssueId;

                            var ErrorMessage = new List<string>();
                            var AdultMembersList = new List<FamilyMembersData>();
                            var MinorMembersList = new List<FamilyMembersData>();
                            var MembershipAcceptedDataList = _dbContext.MembershipAcceptedDatas.Where(i => i.Active).ToList();

                            if ( model.FamilyData!= null && model.FamilyData.Any())
                            {
                                foreach (var item in model.FamilyData)
                                {
                                    var GenderId = new long();
                                    var DobData = GenerateDobFromCivilId(item.CivilId);
                                    if (DobData.transactionStatus != HttpStatusCode.OK)
                                    {
                                        ErrorMessage.Add("CivilId Given For Member " + item.Name + "is Invalid");
                                    }
                                    else if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId || u.PassportNo == item.PassportNo)))
                                    {
                                        if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId)))
                                        {
                                            retModel.returnMessage = "Civil ID Given For the Family Member " + item.Name + " " + "Already Exists";
                                        }
                                        //else if ((MembershipAcceptedDataList.Any(u => u.ContactNo == item.MobileNoRelative)) || (memberShipRequestList.Any(u => u.ContactNo == item.MobileNoRelative)))
                                        //{
                                        //    retModel.returnMessage = "Contact No Given For Family Member " + item.Name + " " + "Already Exists";
                                        //}
                                        else if ((MembershipAcceptedDataList.Any(u => u.PassportNo == item.PassportNo)))
                                        {
                                            retModel.returnMessage = "Passport No Given For Family Member " + item.Name + " " + "Already Exists";
                                        }
                                        retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                                        return retModel;
                                    }
                                    else
                                    {
                                        item.DateOfBirth = DobData.returnData;
                                    }
                                    var relationName = _dbContext.LookupMasters.FirstOrDefault(r => r.LookUpId == item.RelationType)?.LookUpName;

                                    if (relationName != null)
                                    {
                                        GenderId = GetGenderId(relationName.ToLower()) ?? 0;
                                    }

                                    item.GenderId = GenderId;
                                    item.ParentId = Member.IssueId;

                                    var MemberAge = DateTime.UtcNow.Year - item.DateOfBirth.Value.Year;
                                    if (MemberAge > 18)
                                    {
                                        AdultMembersList.Add(item);
                                    }
                                    else if (MemberAge < 18)
                                    {
                                        MinorMembersList.Add(item);
                                    }
                                }

                                if (AdultMembersList.Any())
                                {
                                    var MembersList = AdultMembersList.Select(f => new MembershipDetails
                                    {
                                        Name = f.Name,
                                        CivilId = f.CivilId,
                                        PassportNo = f.PassportNo,
                                        DateofBirth = f.DateOfBirth,
                                        GenderId = f.GenderId,
                                        BloodGroupId = f.BloodGroupid,
                                        ProffessionId = f.Professionid,
                                        CountryCode = f.CountryCodeid,
                                        ContactNo = f.MobileNoRelative,
                                        Email = f.EmailRelative,
                                        AreaId = model.AreaId,
                                        Company = f.CompanyName,
                                        KuwaitAddres = model.KuwaitAddress,
                                        PermenantAddress = model.PermenantAddress,
                                        Pincode = model.Pincode,
                                        ParentId = f.ParentId,
                                        Active = true,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = loggedInUser,
                                    }).ToList();

                                    _dbContext.MembershipRequestDetails.AddRange(MembersList);
                                    _dbContext.SaveChanges();
                                }
                                if (MinorMembersList.Any())
                                {
                                    var MembersList = MinorMembersList.Select(f => new MinorApplicantsAcceptedData
                                    {
                                        Name = f.Name,
                                        CivilId = f.CivilId,
                                        RelationType = f.RelationType,
                                        PassportNo = f.PassportNo,
                                        DateofBirth = f.DateOfBirth,
                                        GenderId = f.GenderId,
                                        BloodGroupId = f.BloodGroupid,
                                        ProffessionId = f.Professionid,
                                        CountryCode = model.CountryCodeId,
                                        ContactNo = model.ContactNo,
                                        Email = model.Email,
                                        AreaId = model.AreaId,
                                        KuwaitAddres = model.KuwaitAddress,
                                        PermenantAddress = model.PermenantAddress,
                                        Pincode = model.Pincode,
                                        ParentId = f.ParentId,
                                        Active = true,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = loggedInUser,
                                    }).ToList();

                                    _dbContext.MinorApplicantsAcceptedDatas.AddRange(MembersList);
                                    _dbContext.SaveChanges();
                                }
                            }

                            if (model.Attachment != null)
                            {
                                var docUpload = await SaveAttachment(model.Attachment, Member.IssueId, loggedInUser);
                            }

                            var membershipFee = new MembershipFee
                            {
                                MemberID = model.IssueId,
                                Campaign = model.CampaignId,
                                AmountToPay = _dbContext.Campaigns.Where(c => c.CampaignId == model.CampaignId).Select(c => c.MemberShipFee).FirstOrDefault(),
                                Active = true,
                                CreatedBy = loggedInUser,
                                CreatedDate = DateTime.UtcNow
                            };

                            await _dbContext.MembershipFees.AddAsync(membershipFee);
                            await _dbContext.SaveChangesAsync();
                            model.MembershipFeeId = membershipFee.MembershipFeeId;

                            if (model.AmountRecieved > 0)
                            {
                                var feeResult = await _feeCollectionReport.FeeCollection(model);
                                retModel.returnData = model;
                                retModel.transactionStatus = feeResult.transactionStatus;
                                retModel.returnMessage = feeResult.returnMessage;
                            }
                            else
                            {
                                retModel.returnData = model;
                                retModel.transactionStatus = HttpStatusCode.OK;
                                retModel.returnMessage = "Registered Successfully (No Payment Collected)";
                            }
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            // ❌ Rollback everything
                            await transaction.RollbackAsync();
                            throw; // Let outer catch handle response
                        }
                    }
                    else
                    {
                        // If already issued, just collect fee
                        var feeResult = await _feeCollectionReport.FeeCollection(model);
                        retModel.returnData = feeResult.returnData;
                        retModel.transactionStatus = feeResult.transactionStatus;
                        retModel.returnMessage = feeResult.returnMessage;
                    }
                }
                else
                {
                    var Member = new MembershipRejected
                    {
                        ReferanceNo = GenerateReferanceNumber(false),
                        Name = model.Name,
                        CivilId = model.CivilId,
                        DateofBirth = model.DateofBirth,
                        PassportNo = model.PassportNo,
                        GenderId = model.GenderId,
                        BloodGroupId = model.BloodGroupId,
                        CountryCodeId = model.CountryCodeId,
                        WhatsAppNoCountryCodeid = model.WhatsAppNoCountryCodeid,
                        WhatsAppNo = model.WhatsAppNo,
                        ContactNo = model.ContactNo,
                        Email = model.Email,
                        ProfessionId = model.ProfessionId,
                        ProffessionOther = model.ProffessionOther,
                        Company = model.Company,
                        AreaId = model.AreaId,
                        KuwaitAddres = model.KuwaitAddress,
                        PermenantAddress = model.PermenantAddress,
                        Pincode = model.Pincode,
                        EmergencyContactName = model.EmergencyContactName,
                        EmergencyContactRelation = model.EmergencyContactRelation,
                        EmergencyContactCountryCodeid = model.EmergencyContactCountryCodeid,
                        EmergencyContactNumber = model.EmergencyContactNumber,
                        EmergencyContactEmail = model.EmergencyContactEmail,
                        CampaignId = model.CampaignId,
                        CampaignAmount = model.CampaignAmount,
                        AmountRecieved = model.AmountRecieved,
                        PaymentTypeId = model.PaymentTypeId,
                        PaymentRemarks = model.PaymentRemarks,
                        RejectionReason = model.RejectionReason,
                        RejectionRemarks = model.RejectionRemarks,
                        Active = true,
                        RejectionReasonId = model.RejectionReasonId,
                    };

                    if (model.FamilyData != null && model.FamilyData.Any())
                    {
                        var MembersList = model.FamilyData.Select(f => new MinorApplicantRejectedData
                        {
                            Name = f.Name,
                            CivilId = f.CivilId,
                            RelationType = f.RelationType,
                            PassportNo = f.PassportNo,
                            DateofBirth = f.DateOfBirth,
                            GenderId = f.GenderId,
                            BloodGroupId = f.BloodGroupid,
                            CountryCode = model.CountryCodeId,
                            ContactNo = model.ContactNo,
                            Email = model.Email,
                            AreaId = model.AreaId,
                            KuwaitAddres = model.KuwaitAddress,
                            PermenantAddress = model.PermenantAddress,
                            Pincode = model.Pincode,
                            ParentId = Member.IssueId,
                            Active = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser,
                        }).ToList();

                        _dbContext.MinorApplicantRejectedDatas.AddRange(MembersList);
                        _dbContext.SaveChanges();
                    }

                    await _dbContext.MembershipRejectedDatas.AddAsync(Member);
                    await _dbContext.SaveChangesAsync();
                    model.IssueId = Member.IssueId;

                    retModel.returnData = model;
                    retModel.transactionStatus = HttpStatusCode.OK;
                    retModel.returnMessage = "Membership Rejected";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = $"Server Error: {ex.Message}";
            }

            return retModel;
        }

        public string GenerateReferanceNumber(bool Type)
        {
            var depReferanceNo = "";
            if (Type)
            {
                var lastDepData = _dbContext.MembershipAcceptedDatas.OrderByDescending(i => i.IssueId).FirstOrDefault();
                if (lastDepData != null)
                {
                    var lastNumber = lastDepData != null ? int.Parse(lastDepData.ReferanceNo.Substring(1)) : 0;
                    lastNumber++;
                    return $"M{lastNumber:0000}";
                }
                else
                {
                    return $"M0001";
                }
            }
            else
            {
                var lastDepData = _dbContext.MembershipRejectedDatas.OrderByDescending(i => i.IssueId).FirstOrDefault();
                if (lastDepData != null)
                {
                    var lastNumber = lastDepData != null ? int.Parse(lastDepData.ReferanceNo.Substring(4)) : 0;
                    lastNumber++;
                    return $"RM{lastNumber:0000}";
                }
                else
                {
                    return $"RM0001";
                }
            }

        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllAcceptedMembers(MemberListFilter InputData)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();
            try
            {
                var objModel = new List<PostMembershipViewModel>();

                var retData = _dbContext.MembershipAcceptedDatas.Where(c => c.Active == true)
                    .Include(c => c.AreaData)
                    .Include(c => c.Unit)
                    .Include(c => c.Campaign)
                    .Include(c => c.Profession)
                    .Include(c => c.WorkPlace)

                    .Include(c => c.Users).AsQueryable();

                var departmentDict = _dbContext.Departments.ToDictionary(d => d.DepartmentId, d => d.DepartmentName);
                var lookupDict = _dbContext.LookupMasters.ToDictionary(l => l.LookUpId, l => l.LookUpName);
                var deviceCounts = _dbContext.DeviceDetails.AsNoTracking().Where(d => !string.IsNullOrEmpty(d.CivilId)).GroupBy(d => d.CivilId.Trim())
                    .Select(g => new
                    {
                        CivilId = g.Key,
                        DeviceCount = g.Select(x => x.DeviceId).Distinct().Count()
                    }).ToDictionary(x => x.CivilId, x => x.DeviceCount);

                #region filter
                if (InputData.AreaId.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.AreaId == InputData.AreaId);

                if (InputData.ProffesionId.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.ProfessionId == InputData.ProffesionId);

                if (InputData.WorkplaceId.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.WorkPlaceId == InputData.WorkplaceId);

                if (InputData.Gender.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.GenderId == InputData.Gender);

                if (InputData.BloodGroup.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.BloodGroupId == InputData.BloodGroup);

                if (InputData.District.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.DistrictId == InputData.District);

                if (InputData.Department.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.DepartmentId == InputData.Department);

                if (InputData.Workingsince.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.WorkYear == InputData.Workingsince);

                if (InputData.Zone.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.ZoneId == InputData.Zone);

                if (InputData.Unit.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.UnitId == InputData.Unit);

                if (InputData.CampaignId.GetValueOrDefault() != 0)
                    retData = retData.Where(c => c.CampaignId == InputData.CampaignId);

                if (InputData.PaymentSatus.GetValueOrDefault() != 0)
                {
                    if (InputData.PaymentSatus.GetValueOrDefault() == 1)
                    {
                        retData = retData.Where(c => (c.AmountRecieved ?? 0) > 0);
                    }
                    else if (InputData.PaymentSatus.GetValueOrDefault() == 2)
                    {
                        retData = retData.Where(c => (c.AmountRecieved ?? 0) == 0);
                    }
                }

                if (InputData.AgeGroupId.GetValueOrDefault() != 0)
                {
                    var today = DateTime.Today;
                    int minAge = 0, maxAge = 0;

                    switch (InputData.AgeGroupId)
                    {
                        case 1: minAge = 20; maxAge = 30; break;
                        case 2: minAge = 31; maxAge = 40; break;
                        case 3: minAge = 41; maxAge = 50; break;
                        case 4: minAge = 51; maxAge = 60; break;
                        case 5: minAge = 61; maxAge = 120; break;
                    }

                    var maxDOB = today.AddYears(-minAge);
                    var minDOB = today.AddYears(-maxAge - 1).AddDays(1);

                    retData = retData.Where(c =>
                        c.DateofBirth.HasValue &&
                        c.DateofBirth.Value >= minDOB &&
                        c.DateofBirth.Value <= maxDOB);
                }

                if (!string.IsNullOrEmpty(InputData.SearchColumn) && !string.IsNullOrEmpty(InputData.SearchString))
                {
                    string query = $"{InputData.SearchColumn}.ToLower().Trim().Contains(@0)";
                    retData = retData.Where(query, InputData.SearchString.ToLower().Trim());
                }

                if (InputData.Type == 1)
                {
                    retData = retData.Where(i => deviceCounts.Keys.Contains(i.CivilId));
                }

                if (InputData.Pagesize != null)
                {
                    retModel.TotalCount = retData.Count();
                    var Pagenumber = Convert.ToInt32(InputData.Pagenumber);
                    var Pagesize = Convert.ToInt32(InputData.Pagesize);
                    retData = retData.Skip((Pagenumber - 1) * Pagesize).Take(Pagesize);
                }
                #endregion filter

                var issueIds = retData.Select(i => i.IssueId).ToList(); // get IDs in memory first

                var MembershipFeeData = from a in _dbContext.MembershipFees
                                        where issueIds.Contains((long)a.MemberID) && a.PaidAmount.HasValue
                                        join b in _dbContext.Campaigns on a.Campaign equals b.CampaignId
                                        select new
                                        {
                                            Id = a.MembershipFeeId,
                                            CampaignName = b.CampaignName,
                                            MemberId = a.MemberID
                                        };

                var Model = retData.Select(c => new PostMembershipViewModel()
                {
                    IssueId = c.IssueId,
                    ReferenceNumber = c.ReferanceNo,
                    MembershipNo = c.MembershipNo,
                    Name = c.Name,
                    CivilId = c.CivilId,
                    PassportNo = c.PassportNo,
                    DateofBirth = c.DateofBirth,
                    GenderId = c.GenderId,
                    Gender = c.GenderId != null ? lookupDict.GetValueOrDefault(c.GenderId.Value) : null,
                    BloodGroupId = c.BloodGroupId,
                    BloodGroup = c.BloodGroupId != null ? lookupDict.GetValueOrDefault(c.BloodGroupId.Value) : null,
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.DepartmentId != null ? departmentDict.GetValueOrDefault(c.DepartmentId.Value) : null,
                    ProfessionId = c.ProfessionId,
                    Profession = c.ProfessionId != null ? c.Profession.ProffessionName : null,
                    WorkPlaceId = c.WorkPlaceId,
                    WorkPlace = c.WorkPlaceId != null ? c.WorkPlace.WorkPlaceName : null,
                    PhoneNo = "+" + c.CountryCodeId + c.ContactNo,
                    Email = c.Email,
                    District = c.DistrictId != null ? lookupDict.GetValueOrDefault(c.DistrictId.Value) : null,
                    Unit = c.UnitId != null ? c.Unit.UnitName : null,
                    Area = c.AreaId != null ? c.AreaData.AreaName : null,
                    CreatedDate = c.CreatedDate,
                    Active = c.Active,
                    PaymentDone = (c.AmountRecieved ?? 0) > 0,
                    DeviceCount = deviceCounts.GetValueOrDefault((c.CivilId ?? string.Empty).Trim(), 0),
                    Age = c.DateofBirth.HasValue ? (int)((DateTime.Today - c.DateofBirth.Value).TotalDays / 365.25) : (int?)null,
                    Memberfrom = c.Memberfrom,
                    MemberfromString = c.CreatedDate != null ? c.CreatedDate.Value.Date.ToString("dd-MM-yyyy") : null,
                    CampaignEndDateString = c.CampaignId != null ? c.Campaign.EndDate.Value.Date.ToString("dd-MM-yyyy") : null,
                    LastMembershipAdded = MembershipFeeData.Any(i => i.MemberId == c.IssueId) ? MembershipFeeData.OrderByDescending(e => e.Id).FirstOrDefault(i => i.MemberId == c.IssueId).CampaignName : "N/A"
                });

                objModel = Model.OrderBy(i => i.Name).ToList();

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

        public ResponseEntity<List<PostMembershipViewModel>> GetAllRejectedMembers(long? Areaid, long? Proffesionid, long? workplaceId)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();
            try
            {
                var objModel = new List<PostMembershipViewModel>();

                // Base query: Active users
                var retData = _dbContext.MembershipRejectedDatas.Where(c => c.Active == true);

                // Filter by areaid
                if (Areaid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.AreaId == Areaid);
                }
                if (Proffesionid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.ProfessionId == Proffesionid);
                }
                if (workplaceId.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.WorkPlaceId == workplaceId);
                }


                // Map to ViewModel
                objModel = retData.Select(c => new PostMembershipViewModel()
                {
                    IssueId = c.IssueId,
                    ReferenceNumber = c.ReferanceNo,
                    Name = c.Name,
                    CivilId = c.CivilId,
                    PassportNo = c.PassportNo,
                    DateofBirth = c.DateofBirth,
                    GenderId = c.GenderId,
                    Gender = _dbContext.LookupMasters
                        .Where(r => r.LookUpId == c.GenderId)
                        .Select(r => r.LookUpName)
                        .FirstOrDefault(),
                    BloodGroupId = c.BloodGroupId,
                    BloodGroup = _dbContext.LookupMasters
                        .Where(e => e.LookUpId == c.BloodGroupId)
                        .Select(e => e.LookUpName)
                        .FirstOrDefault(),
                    ProfessionId = c.ProfessionId,
                    Profession = _dbContext.Professions
                        .Where(e => e.ProfessionId == c.ProfessionId)
                        .Select(e => e.ProffessionName)
                        .FirstOrDefault(),
                    WorkPlaceId = c.WorkPlaceId,
                    WorkPlace = _dbContext.WorkPlace
                        .Where(e => e.WorkPlaceId == c.WorkPlaceId)
                        .Select(e => e.WorkPlaceName)
                        .FirstOrDefault(),
                    PhoneNo = "+" + c.CountryCodeId + c.ContactNo,
                    Email = c.Email,
                    District = _dbContext.LookupMasters
                        .Where(e => e.LookUpId == c.DistrictId)
                        .Select(e => e.LookUpName)
                        .FirstOrDefault(),
                    Area = _dbContext.AreaDatas
                        .Where(e => e.AreaId == c.AreaId)
                        .Select(e => e.AreaName)
                        .FirstOrDefault(),
                    DepartmentId = c.DepartmentId,
                    DepartmentName = _dbContext.Departments
                        .Where(e => e.DepartmentId == c.DepartmentId)
                        .Select(e => e.DepartmentName)
                        .FirstOrDefault(),
                    Unit = _dbContext.Units
                        .Where(e => e.UnitId == c.UnitId)
                        .Select(e => e.UnitName)
                        .FirstOrDefault(),
                    CreatedDate = c.CreatedDate,
                    Active = c.Active
                }).ToList();

                // Set response
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderBy(i => i.Name).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }

            return retModel;
        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllCancelledMembers(long? Areaid, long? Proffesionid, long? workplaceId)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();
            try
            {
                var objModel = new List<PostMembershipViewModel>();

                // Base query: Active users
                var retData = _dbContext.MemberShipCancelledDatas.Where(c => c.Active == true);

                // Filter by areaid
                if (Areaid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.AreaId == Areaid);
                }
                if (Proffesionid.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.ProfessionId == Proffesionid);
                }
                if (workplaceId.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.WorkPlaceId == workplaceId);
                }


                // Map to ViewModel
                objModel = retData.Select(c => new PostMembershipViewModel()
                {
                    IssueId = c.IssueId,
                    ReferenceNumber = c.ReferanceNo,
                    Name = c.Name,
                    CivilId = c.CivilId,
                    PassportNo = c.PassportNo,
                    DateofBirth = c.DateofBirth,
                    GenderId = c.GenderId,
                    Gender = _dbContext.LookupMasters
                        .Where(r => r.LookUpId == c.GenderId)
                        .Select(r => r.LookUpName)
                        .FirstOrDefault(),
                    BloodGroupId = c.BloodGroupId,
                    BloodGroup = _dbContext.LookupMasters
                        .Where(e => e.LookUpId == c.BloodGroupId)
                        .Select(e => e.LookUpName)
                        .FirstOrDefault(),
                    ProfessionId = c.ProfessionId,
                    Profession = _dbContext.Professions
                        .Where(e => e.ProfessionId == c.ProfessionId)
                        .Select(e => e.ProffessionName)
                        .FirstOrDefault(),
                    //WorkPlaceId = c.WorkPlaceId,
                    //WorkPlace = _dbContext.WorkPlace
                    //    .Where(e => e.WorkPlaceId == c.WorkPlaceId)
                    //    .Select(e => e.WorkPlaceName)
                    //    .FirstOrDefault(),
                    PhoneNo = "+" + c.CountryCode + c.ContactNo,
                    Email = c.Email,
                    District = _dbContext.LookupMasters
                        .Where(e => e.LookUpId == c.DistrictId)
                        .Select(e => e.LookUpName)
                        .FirstOrDefault(),
                    Area = _dbContext.AreaDatas
                        .Where(e => e.AreaId == c.AreaId)
                        .Select(e => e.AreaName)
                        .FirstOrDefault(),
                    DepartmentName = _dbContext.Departments
                        .Where(e => e.DepartmentId == c.DepartmentId)
                        .Select(e => e.DepartmentName)
                        .FirstOrDefault(),
                    Unit = _dbContext.Units
                        .Where(e => e.UnitId == c.UnitId)
                        .Select(e => e.UnitName)
                        .FirstOrDefault(),
                    CreatedDate = c.CreatedDate,
                    Active = c.Active
                }).ToList();

                // Set response
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderBy(i => i.Name).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }

            return retModel;
        }

        public ResponseEntity<string> ExportCancelledMemberstoExcel(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllCancelledMembers(Area, 0, 0);


                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();

                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Cancelled Members List");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Member ID";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Civil ID";
                        worksheet.Cell(1, 5).Value = "Contact No";
                        worksheet.Cell(1, 6).Value = "Email";
                        worksheet.Cell(1, 7).Value = "Profession";
                        worksheet.Cell(1, 8).Value = "Workplace";
                        worksheet.Cell(1, 9).Value = "Department";
                        worksheet.Cell(1, 10).Value = "Area";
                        worksheet.Cell(1, 11).Value = "Unit";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(i + 2, 3).Value = item.Name;
                            worksheet.Cell(i + 2, 4).Value = item.CivilId;
                            worksheet.Cell(i + 2, 5).Value = item.PhoneNo;
                            worksheet.Cell(i + 2, 6).Value = item.Email;
                            worksheet.Cell(i + 2, 7).Value = item.Profession;
                            worksheet.Cell(i + 2, 8).Value = item.WorkPlace;
                            worksheet.Cell(i + 2, 9).Value = item.DepartmentName;
                            worksheet.Cell(i + 2, 10).Value = item.Area;
                            worksheet.Cell(i + 2, 11).Value = item.Unit;
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

        public ResponseEntity<string> ExportCancelledMemberstoPdf(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllCancelledMembers(Area, 0, 0); // Adjust filters as needed

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();
                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate PDF using iTextSharp
                    using (var ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        FixITextSharpVersionBug(); // call this before GetInstance()
                        PdfWriter.GetInstance(document, ms);
                        document.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                        Paragraph title = new Paragraph("Cancelled Members List", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 15f
                        };
                        document.Add(title);

                        PdfPTable table = new PdfPTable(11)
                        {
                            WidthPercentage = 100
                        };
                        table.SetWidths(new float[] { 3f, 6f, 7f, 7f, 7f, 7f, 10f, 10f, 10f, 7f, 7f });

                        // Header row
                        string[] headers = { "Sl No", "Membership ID", "Full Name", "Civil ID", "Contact No", "Email", "Profession", "Workplace", "Department", "Area", "Unit" };


                        foreach (var header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                            table.AddCell(cell);
                        }

                        // Add rows
                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            table.AddCell(new Phrase((i + 1).ToString(), cellFont));
                            table.AddCell(new Phrase(item.ReferenceNumber ?? "", cellFont));
                            table.AddCell(new Phrase(item.Name ?? "", cellFont));
                            table.AddCell(new Phrase(item.CivilId ?? "", cellFont));
                            table.AddCell(new Phrase(item.PhoneNo ?? "", cellFont));
                            table.AddCell(new Phrase(item.Email ?? "", cellFont));
                            table.AddCell(new Phrase(item.Profession ?? "", cellFont));
                            table.AddCell(new Phrase(item.WorkPlace ?? "", cellFont));
                            table.AddCell(new Phrase(item.DepartmentName ?? "", cellFont));

                            table.AddCell(new Phrase(item.Area ?? "", cellFont));
                            table.AddCell(new Phrase(item.Unit ?? "", cellFont));
                        }

                        document.Add(table);
                        document.Close();

                        byte[] pdfBytes = ms.ToArray();

                        // Save or convert to Base64 or link etc. via your utility
                        retModel.returnData = GenericUtilities.SetReportData(pdfBytes, ".pdf");
                        retModel.transactionStatus = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Error generating PDF: " + ex.Message;
            }

            return retModel;
        }

        public async Task<ResponseEntity<bool>> IssueMemberFromRegister(PostMembershipViewModel model)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                if (model.IssueId == null)
                {

                    var MemberExists = _dbContext.MembershipAcceptedDatas
                            .Any(u => u.CivilId == model.CivilId && u.ContactNo == model.ContactNo);
                    if (MemberExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                        retModel.returnMessage = "Member Already Exist";
                    }
                    else
                    {
                        var MemberData = GetMemberById(model.MembershipId);
                        var Member = new PostMembershipViewModel
                        {
                            Name = MemberData.returnData.Name,
                            CivilId = MemberData.returnData.CivilId,
                            DateofBirth = MemberData.returnData.DateofBirth,
                            PassportNo = MemberData.returnData.PassportNo,
                            GenderId = MemberData.returnData.GenderId,
                            BloodGroupId = MemberData.returnData.BloodGroupId,
                            CountryCodeId = MemberData.returnData.CountryCodeId,
                            ContactNo = MemberData.returnData.ContactNo,
                            WhatsAppNoCountryCodeid = MemberData.returnData.WhatsAppNoCountryCodeid,
                            WhatsAppNo = MemberData.returnData.WhatsAppNo,
                            Email = MemberData.returnData.Email,
                            ProfessionId = MemberData.returnData.ProfessionId,
                            Company = MemberData.returnData.Company,
                            AreaId = MemberData.returnData.AreaId,
                            KuwaitAddress = MemberData.returnData.KuwaitAddress,
                            MembershipType = MemberData.returnData.MembershipType,
                            FamilyData = MemberData.returnData.FamilyData,
                            PermenantAddress = MemberData.returnData.PermenantAddress,
                            Pincode = MemberData.returnData.Pincode,
                            EmergencyContactName = MemberData.returnData.EmergencyContactName,
                            EmergencyContactRelation = MemberData.returnData.EmergencyContactRelation,
                            EmergencyContactCountryCodeid = MemberData.returnData.EmergencyContactCountryCodeid,
                            EmergencyContactNumber = MemberData.returnData.EmergencyContactNumber,
                            EmergencyContactEmail = MemberData.returnData.EmergencyContactEmail,
                            ZoneId = model.ZoneId,
                            UnitId = model.UnitId,
                            ReferredBy = model.ReferredBy,
                            CampaignId = model.CampaignId,
                            CampaignAmount = _dbContext.Campaigns.Where(c => c.CampaignId == model.CampaignId).Select(c => c.MemberShipFee).FirstOrDefault(),
                            AmountRecieved = model.AmountRecieved,
                            PaymentTypeId = model.PaymentTypeId,
                            PaymentRemarks = model.PaymentRemarks,
                            MembershipStatus = model.MembershipStatus,
                            RejectionReasonId = model.RejectionReasonId,
                            MembershipRequestedDate = MemberData.returnData.CreatedDate,
                            RejectionReason = model.RejectionReason,
                            RejectionRemarks = model.RejectionRemarks,
                            ProffessionOther = MemberData.returnData.ProffessionOther
                        };

                        var IsMemberIssued = await IssueMember(Member);
                        if (IsMemberIssued.transactionStatus != HttpStatusCode.OK)
                        {
                            retModel.returnMessage = IsMemberIssued.returnMessage;
                            retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                        }
                        else
                        {
                            var IsDeleteApproved = false;
                            if (Member.MembershipStatus == 1)
                            {
                                var IsMemberAccepted = _dbContext.MembershipAcceptedDatas.FirstOrDefault(i => i.IssueId == IsMemberIssued.returnData.IssueId);
                                if (IsMemberIssued.transactionStatus == HttpStatusCode.OK && IsMemberAccepted.CivilId == Member.CivilId && IsMemberAccepted.ContactNo == Member.ContactNo && IsMemberAccepted.PassportNo == Member.PassportNo)
                                {
                                    IsDeleteApproved = true;
                                }
                            }
                            else
                            {
                                var IsMemberRejected = _dbContext.MembershipRejectedDatas.FirstOrDefault(i => i.IssueId == IsMemberIssued.returnData.IssueId);
                                if (IsMemberIssued.transactionStatus == HttpStatusCode.OK && IsMemberRejected.CivilId == Member.CivilId && IsMemberRejected.ContactNo == Member.ContactNo && IsMemberRejected.PassportNo == Member.PassportNo)
                                {
                                    IsDeleteApproved = true;
                                }
                            }

                            if (IsDeleteApproved)
                            {
                                var membership = _dbContext.MembershipRequestDetails.FirstOrDefault(i => i.MembershipId == model.MembershipId);

                                if (membership != null)
                                {
                                    _dbContext.MembershipRequestDetails.Remove(membership);
                                    var FamilyMembersID = Member.FamilyData != null ?  Member.FamilyData.Select(i => i.MembershipId).ToList() : null;
                                    if(FamilyMembersID != null)
                                    {
                                        var minorsToRemove = _dbContext.MinorApplicantDetails.Where(i => FamilyMembersID.Contains(i.MembershipId)).ToList();
                                        _dbContext.MinorApplicantDetails.RemoveRange(minorsToRemove);
                                    }
                                    _dbContext.SaveChanges();
                                }
                                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                                retModel.returnMessage = model.MembershipStatus == 1 ? "Membership Issued" : "Membership Rejected";
                            }
                            else
                            {
                                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                                retModel.returnMessage = "Server Error";
                            }
                        }

                    }
                }
                else
                {
                    // If already issued, just collect fee
                    var feeResult = await _feeCollectionReport.FeeCollection(model);
                    //retModel.returnData = feeResult.;
                    retModel.transactionStatus = feeResult.transactionStatus;
                    retModel.returnMessage = feeResult.returnMessage;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public ResponseEntity<PostMembershipViewModel> GetAcceptedMemberById(long? id)
        {
            var retModel = new ResponseEntity<PostMembershipViewModel>();
            try
            {
                TimeZoneInfo kuwaitTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

                var ObjData = _dbContext.MembershipAcceptedDatas
                    .Include(c => c.Profession)
                    .Include(c => c.WorkPlace)
                    .Include(c => c.Unit)
                    .Include(c => c.Zone)
                    .Include(c => c.AreaData)
                    .Include(c => c.Campaign)
                    .FirstOrDefault(u => u.IssueId == id);
                var LookUpMasterData = _dbContext.LookupMasters.Where(i => i.Active).ToList();
                var ProfileData = _dbContext.MemberProfileDatas.Any(i => i.MemberId == id) ? _dbContext.MemberProfileDatas.Include(c => c.FileStorage).OrderByDescending(i => i.AttachmentId).FirstOrDefault(i => i.MemberId == id) : null;
                var retData = new PostMembershipViewModel();
                if (ObjData != null)
                {
                    retData = new PostMembershipViewModel
                    {
                        IssueId = ObjData.IssueId,
                        ReferenceNumber = ObjData.ReferanceNo,
                        Name = ObjData.Name,
                        CivilId = ObjData.CivilId,
                        PassportNo = ObjData.PassportNo,
                        DateofBirth = ObjData.DateofBirth,
                        GenderId = ObjData.GenderId,
                        Gender = LookUpMasterData.Where(r => r.LookUpId == ObjData.GenderId).Select(r => r.LookUpName).FirstOrDefault(),
                        BloodGroupId = ObjData.BloodGroupId,
                        BloodGroup = LookUpMasterData.Where(r => r.LookUpId == ObjData.BloodGroupId).Select(r => r.LookUpName).FirstOrDefault(),
                        ProfessionId = ObjData.ProfessionId,
                        Profession = ObjData.Profession.ProffessionName,
                        CountryCodeId = ObjData.CountryCodeId,
                        ContactNo = ObjData.ContactNo,
                        WhatsAppNoCountryCodeid = ObjData.WhatsAppNoCountryCodeid,
                        WhatsAppNo = ObjData.WhatsAppNo,
                        PhoneNo = $"+{ ObjData.CountryCodeId} { ObjData.ContactNo}",
                        WhatsappNoString = $"+{ObjData.WhatsAppNoCountryCodeid} {ObjData.WhatsAppNo}",
                        Email = ObjData.Email,
                        AreaId = (long)ObjData.AreaId,
                        ZoneId = ObjData.ZoneId,
                        UnitId = ObjData.UnitId,
                        Area = ObjData.AreaData.AreaName,
                        Zone = ObjData.Zone.ZoneName,
                        Unit = ObjData.Unit.UnitName,
                        CampaignId = ObjData.CampaignId,
                        CampaignName = ObjData.Campaign.CampaignName,
                        CampaignAmount = ObjData.CampaignAmount,
                        AmountRecieved = ObjData.AmountRecieved,
                        PaymentTypeId = ObjData.PaymentTypeId,
                        PaymentType = LookUpMasterData.Where(r => r.LookUpId == ObjData.PaymentTypeId).Select(r => r.LookUpName).FirstOrDefault(),
                        PaymentRemarks = ObjData.PaymentRemarks,
                        DateofBirthString = ObjData.DateofBirth != null ? ObjData.DateofBirth.Value.Date.ToString("dd-MM-yyyy") : null,
                        Active = ObjData.Active,
                        MemberfromString = ObjData.Memberfrom != null && ObjData.Memberfrom != DateTime.MinValue ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(ObjData.Memberfrom, GenericUtilities.dateTimeFormat) : "",
                        MembershipRequestedDateString = ObjData.MembershipRequestedDate != null && ObjData.MembershipRequestedDate != DateTime.MinValue ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(ObjData.MembershipRequestedDate, GenericUtilities.dateTimeFormat) : "",
                        ApprovedByName = ObjData.ApprovedBy != null ? _dbContext.Users.FirstOrDefault(i => i.UserId == ObjData.ApprovedBy).UserName : null,
                        ProfileImagePath = ProfileData != null ? ProfileData.FileStorage.FilePath : "/images/Default.jpg",
                        PaymentRecievedBy = ObjData.AmountRecieved != null && ObjData.ApprovedBy != null ? _dbContext.Users.FirstOrDefault(i => i.UserId == ObjData.ApprovedBy).UserName : null,
                        PaymentRecievedDate = ObjData.AmountRecieved != null && ObjData.CreatedDate != null ? ObjData.CreatedDate.Value.Date.ToString("dd-MM-yyyy") : null,
                        PaymentDone = ObjData.AmountRecieved == 0 ? false : true,
                        ReferredByName = ObjData.ReferredBy != null ? _dbContext.MembershipAcceptedDatas.FirstOrDefault(i => i.IssueId == ObjData.ReferredBy)?.Name : null,
                        ProffessionOther = ObjData.ProffessionOther,
                        Company = ObjData.Company,
                        KuwaitAddress = ObjData.KuwaitAddres,
                        PermenantAddress = ObjData.PermenantAddress,
                        Pincode = ObjData.Pincode,
                        EmergencyContactName = ObjData.EmergencyContactName,
                        EmergencyContactRelation = ObjData.EmergencyContactRelation,
                        EmergencyContactRelationString = ObjData.EmergencyContactRelation != null ? LookUpMasterData.SingleOrDefault(i=>i.LookUpId == ObjData.EmergencyContactRelation).LookUpName : null,
                        EmergencyContactNumberString = $"+{ObjData.EmergencyContactCountryCodeid} {ObjData.EmergencyContactNumber}",
                        EmergencyContactEmail = ObjData.EmergencyContactEmail,
                        EmergencyContactCountryCodeid = ObjData.EmergencyContactCountryCodeid,
                        EmergencyContactNumber = ObjData.EmergencyContactNumber,
                        MembershipNo = ObjData.MembershipNo != null ? ObjData.MembershipNo : ObjData.ReferanceNo,
                        ReferredBy = ObjData.ReferredBy,
                    };

                    var AllDeviceData = _dbContext.DeviceDetails.Where(i => i.CivilId.Trim().ToLower() == retData.CivilId.Trim().ToLower()).OrderByDescending(i => i.DeviceDetailId).ToList();

                    if(AllDeviceData.Any())
                    {
                       var DeviceListOfMember = AllDeviceData.DistinctBy(i => i.DeviceId).Select(
                       i => new DeviceDetailViewModel
                       {
                           DeviceDetailId = i.DeviceDetailId,
                           DeviceType = i.DeviceType,
                           DeviceId = i.DeviceId,
                           Token = i.FCMToken,
                           DeviceName = i.DeviceName,
                           DeviceModel = i.DeviceModel,
                           OrgFileName = i.OrgFileName,
                           FilePath = i.FilePath != null ? i.FilePath : "/images/default.jpg",
                           LastOpenDateTimeString = i.LastOpenDateTime != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.LastOpenDateTime, GenericUtilities.dateTimeFormat) : null,
                           LastClosedDateTimeString = i.LastClosedDateTime != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.LastClosedDateTime, GenericUtilities.dateTimeFormat) : null,
                           CreatedDateTimeString = AllDeviceData.Where(k => k.DeviceId == i.DeviceId).Any() ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(AllDeviceData.Where(k => k.DeviceId.Trim().ToLower() == i.DeviceId.Trim().ToLower()).OrderBy(k => k.DeviceDetailId).FirstOrDefault().CreatedDate, GenericUtilities.dateTimeFormat) : null,
                           LogOutDateTimeString = i.LogOutDateTime != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.LogOutDateTime, GenericUtilities.dateTimeFormat) : null,
                           Active = i.Active,
                           ForceLogOut = i.IsForceLogout,
                           HistoryList = AllDeviceData.Where(s => s.DeviceId.Trim().ToLower() == i.DeviceId.Trim().ToLower() && (s.DeviceDetailId != i.DeviceDetailId)).Select
                           (d => new DeviceDetailHistoryList
                           {
                               DeviceName = d.DeviceName,
                               OrgFileName = d.OrgFileName,
                               FilePath = d.FilePath,
                               LastOpenDateTimeString = d.LastOpenDateTime != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(d.LastOpenDateTime, GenericUtilities.dateTimeFormat) : null,
                               LastClosedDateTimeString = d.LastClosedDateTime != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(d.LastClosedDateTime, GenericUtilities.dateTimeFormat) : null,
                               CreatedDateTimeString = d.CreatedDate != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(d.CreatedDate, GenericUtilities.dateTimeFormat) : null,
                               LogOutDateTimeString = d.LogOutDateTime != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(d.LogOutDateTime, GenericUtilities.dateTimeFormat) : null,
                           }).ToList(),
                       }).ToList();
                       retData.DeviceData = DeviceListOfMember;
                    }

                    var MinorRelativeData = _dbContext.MinorApplicantsAcceptedDatas.Where(i => i.Active && i.ParentId == retData.IssueId).ToList();
                    if(MinorRelativeData != null && MinorRelativeData.Any())
                    {
                        var MinorApplicantList = MinorRelativeData.Select(
                            i => new FamilyMembersData
                            {
                                MembershipId = i.MembershipId,
                                Name = i.Name,
                                RelationType = i.RelationType,
                                RelationTypeName = i.RelationType != null ? _dbContext.LookupMasters.SingleOrDefault(r => r.LookUpId == i.RelationType).LookUpName : null,
                                CivilId = i.CivilId,
                                PassportNo = i.PassportNo,
                                DateOfBirth = i.DateofBirth,
                                DateofBirthString = i.DateofBirth != null ? i.DateofBirth.Value.Date.ToString("dd-MM-yyyy") : null,
                                GenderId = i.GenderId,
                                GenderString = i.GenderId != null && i.GenderId != 0 ? _dbContext.LookupMasters.SingleOrDefault(r => r.LookUpId == i.GenderId).LookUpName : null,
                                BloodGroupid = i.BloodGroupId,
                                BloodGroup = i.BloodGroupId != null ? _dbContext.LookupMasters.SingleOrDefault(r => r.LookUpId == i.BloodGroupId).LookUpName : null,
                                CountryCodeid = i.CountryCode,
                                MobileNoRelative = i.ContactNo,
                                MobileNoRelativeString = $"+{i.CountryCode} {i.ContactNo}",
                                EmailRelative = i.Email,
                            }).ToList();
                        retData.FamilyData = MinorApplicantList;
                    }

                    retModel.returnData = retData;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<PostMembershipViewModel> GetRejectedMemberById(long? id)
        {
            var retModel = new ResponseEntity<PostMembershipViewModel>();
            try
            {
                var ObjData = _dbContext.MembershipRejectedDatas
                    .Include(c => c.Profession)
                    .Include(c => c.WorkPlace)
                    .Include(c => c.Unit)
                    .Include(c => c.Zone)
                    .Include(c => c.AreaData)
                    .FirstOrDefault(u => u.IssueId == id);
                var LookUpMasterData = _dbContext.LookupMasters.Where(i => i.Active).ToList();
                var retData = new PostMembershipViewModel();
                if (ObjData != null)
                {
                    retData = new PostMembershipViewModel
                    {
                        ReferenceNumber = ObjData.ReferanceNo,
                        Name = ObjData.Name,
                        CivilId = ObjData.CivilId,
                        PassportNo = ObjData.PassportNo,
                        DateofBirth = ObjData.DateofBirth,
                        GenderId = ObjData.GenderId,
                        Gender = ObjData.GenderId != null ? LookUpMasterData
                            .Where(r => r.LookUpId == ObjData.GenderId)
                            .Select(r => r.LookUpName)
                            .FirstOrDefault() : "",
                        BloodGroupId = ObjData.BloodGroupId,
                        BloodGroup = ObjData.BloodGroupId != null ? LookUpMasterData
                            .Where(r => r.LookUpId == ObjData.BloodGroupId)
                            .Select(r => r.LookUpName)
                            .FirstOrDefault() : "",
                        ProfessionId = ObjData.ProfessionId,
                        Profession = ObjData.Profession.ProffessionName,
                        //WorkPlaceId = ObjData.WorkPlaceId,
                        //WorkPlace = ObjData.WorkPlace.WorkPlaceName,
                        CountryCodeId = ObjData.CountryCodeId,
                        ContactNo = ObjData.ContactNo,
                        Email = ObjData.Email,
                        //DistrictId = (long)ObjData.DistrictId,
                        //District = ObjData.DistrictId != null ? LookUpMasterData.Where(r => r.LookUpId == ObjData.DistrictId).Select(r => r.LookUpName).FirstOrDefault() : "",
                        AreaId = (long)ObjData.AreaId,
                        Area = ObjData.AreaData.AreaName,
                        CampaignId = ObjData.CampaignId,
                        CampaignAmount = ObjData.CampaignAmount,
                        AmountRecieved = ObjData.AmountRecieved,
                        PaymentTypeId = ObjData.PaymentTypeId,
                        PaymentType = ObjData.PaymentTypeId != null ? LookUpMasterData
                            .Where(r => r.LookUpId == ObjData.PaymentTypeId)
                            .Select(r => r.LookUpName)
                            .FirstOrDefault() : "",
                        PaymentRemarks = ObjData.PaymentRemarks,
                        HearAboutUsId = ObjData.HearAboutUsId,
                        HearAboutUs = ObjData.HearAboutUsId != null ? LookUpMasterData
                            .Where(r => r.LookUpId == ObjData.HearAboutUsId)
                            .Select(r => r.LookUpName)
                            .FirstOrDefault() : "",
                        DateofBirthString = ObjData.DateofBirth != null ? ObjData.DateofBirth.Value.Date.ToString("dd-MM-yyyy") : null,
                        Active = ObjData.Active,
                        RejectedDate = ObjData.CreatedDate != null ? ObjData.CreatedDate.Value.Date.ToString("dd-MM-yyyy") : null,
                        RejectionReason = ObjData.RejectionReasonId != null ? LookUpMasterData.Where(r => r.LookUpId == ObjData.RejectionReasonId).Select(r => r.LookUpName).FirstOrDefault() : "",
                        RejectionRemarks = ObjData.RejectionRemarks,
                        PhoneNo = "+" + ObjData.CountryCodeId + ObjData.ContactNo,
                        RejectedByName = ObjData.CreatedBy != null ? _dbContext.Users.FirstOrDefault(i => i.UserId == ObjData.CreatedBy).UserName : null,
                        //WorkYear = ObjData.WorkYear,
                        //DepartmentName = ObjData.DepartmentId != null ? _dbContext.Departments.FirstOrDefault(i => i.DepartmentId == ObjData.DepartmentId).DepartmentName : "",
                        PaymentDone = ObjData.AmountRecieved == 0 ? false : true,
                        ProffessionOther = ObjData.ProffessionOther,
                        //WorkplaceOther = ObjData.WorkplaceOther,
                    };
                    retModel.returnData = retData;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        #endregion Issue_Memebrship    

        #region MemeberShipValidations
        public long IsValidKuwaitCivilID(string civilId)
        {
             civilId = civilId?.Trim();
            if (string.IsNullOrEmpty(civilId) || !Regex.IsMatch(civilId, @"^[1-3]\d{11}$"))
                return 1;

            string dobPart = civilId.Substring(1, 6);
            string yearPrefix;

            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas.Any(u => u.CivilId == civilId);
            var MemberExists = _dbContext.MembershipRequestDetails.Any(u => u.CivilId == civilId);
            if (AcceptedMemberExists)
            {
                return 2;
            }

            switch (civilId[0])
            {
                case '2':
                    yearPrefix = "19";
                    break;
                case '3':
                    yearPrefix = "20";
                    break;
                default:
                    return 1;
            }

            string year = yearPrefix + dobPart.Substring(0, 2);
            string month = dobPart.Substring(2, 2);
            string day = dobPart.Substring(4, 2);

            if (!DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                return 1;

            int[] weights = { 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;

            for (int i = 0; i < 11; i++)
            {
                sum += (civilId[i] - '0') * weights[i];
            }

            int checksum = 11 - (sum % 11);
            if (checksum == 11) checksum = 0;

            if (checksum == 10) return 1;

            return (civilId[11] - '0') == checksum ? 3 : 1;
        }

        public long IsValidateKuwaitCivilID(string civilId, long? membershipId = null)
        {
            if (string.IsNullOrEmpty(civilId) || !Regex.IsMatch(civilId, @"^[1-3]\d{11}$"))
                return 1;

            string dobPart = civilId.Substring(1, 6);
            string yearPrefix;

            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas
                .Any(u => u.CivilId == civilId && (membershipId == null || u.IssueId != membershipId));

            var MemberExists = _dbContext.MembershipRequestDetails
                .Any(u => u.CivilId == civilId && (membershipId == null || u.MembershipId != membershipId));

            if (MemberExists || AcceptedMemberExists)
            {
                return 2; // Already exists for another record
            }

            switch (civilId[0])
            {
                case '2':
                    yearPrefix = "19";
                    break;
                case '3':
                    yearPrefix = "20";
                    break;
                default:
                    return 1;
            }

            string year = yearPrefix + dobPart.Substring(0, 2);
            string month = dobPart.Substring(2, 2);
            string day = dobPart.Substring(4, 2);

            if (!DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                return 1;

            int[] weights = { 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;

            for (int i = 0; i < 11; i++)
            {
                sum += (civilId[i] - '0') * weights[i];
            }

            int checksum = 11 - (sum % 11);
            if (checksum == 11) checksum = 0;
            if (checksum == 10) return 1;

            return (civilId[11] - '0') == checksum ? 3 : 1;



            //switch (civilId[0])
            //{
            //    case '2':
            //        yearPrefix = "19";
            //        break;
            //    case '3':
            //        yearPrefix = "20";
            //        break;
            //    default:
            //        return 1;
            //}

            //string year = yearPrefix + dobPart.Substring(0, 2);
            //string month = dobPart.Substring(2, 2);
            //string day = dobPart.Substring(4, 2);

            //if (!DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
            //    return 1;

            //int[] weights = { 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //int sum = 0;

            //for (int i = 0; i < 11; i++)
            //{
            //    sum += (civilId[i] - '0') * weights[i];
            //}

            //int checksum = 11 - (sum % 11);
            //if (checksum == 11) checksum = 0;

            //if (checksum == 10) return 1;

            //return (civilId[11] - '0') == checksum ? 3 : 1;
        }

        public long IsValidateEditIssuePassportNo(string passportNo, long? membershipId = null)
        {
            if (string.IsNullOrWhiteSpace(passportNo))
                return 1; // Invalid - empty

            passportNo = passportNo.Trim().ToUpper();

            // Must be exactly 8 characters, e.g., "A1234567"
            var pattern = @"^[A-Z][0-9]{7}$";
            if (!Regex.IsMatch(passportNo, pattern))
                return 1; // Invalid format

            // Check uniqueness - ignore same issueId in edit mode
            bool acceptedExists = _dbContext.MembershipAcceptedDatas
                .Any(x => x.PassportNo == passportNo && (membershipId == null || x.IssueId != membershipId));

            bool requestExists = _dbContext.MembershipRequestDetails
                .Any(x => x.PassportNo == passportNo && (membershipId == null || x.MembershipId != membershipId));

            if (acceptedExists || requestExists)
                return 2; // Already exists

            return 3; // ✅ Valid
        }

        public bool IsValidEditIssueContactNo(long contactNo, long? membershipId = null)
        {
            // Check if contact exists in Accepted Members (excluding current issueId if provided)
            bool acceptedExists = _dbContext.MembershipAcceptedDatas
                .Any(u => u.ContactNo == contactNo && (membershipId == null || u.IssueId != membershipId));

            // Check if contact exists in Request Details (excluding current issueId if provided)
            bool requestExists = _dbContext.MembershipRequestDetails
                .Any(u => u.ContactNo == contactNo && (membershipId == null || u.MembershipId != membershipId));

            // Return true only if no duplicates found
            return !(acceptedExists || requestExists);
        }

        public long IsValidatePassportNo(string PassportNo)
        {
            if (string.IsNullOrWhiteSpace(PassportNo)) return 1;
            if (PassportNo.Length != 8) return 1;

            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas.Any(u => u.PassportNo == PassportNo);
            //var MemberExists = _dbContext.MembershipRequestDetails.Any(u => u.PassportNo == PassportNo);
            if (AcceptedMemberExists)
            {
                return 2;
            }
            var pattern = @"^[A-Z][0-9]\d{5}[0-9]$";
            return Regex.IsMatch(PassportNo, pattern) ? 3 : 1;
        }

        public bool IsValidContactNo(long ContactNo)
        {
            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas.Any(u => u.ContactNo == ContactNo);
            //var MemberExists = _dbContext.MembershipRequestDetails.Any(u => u.ContactNo == ContactNo);
            if (AcceptedMemberExists)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public long IsValidKuwaitCivilIDforEdit(string civilId, long? issueId)
        {
            if (string.IsNullOrEmpty(civilId) || !Regex.IsMatch(civilId, @"^[1-3]\d{11}$"))
                return 1;

            string dobPart = civilId.Substring(1, 6);
            string yearPrefix;

            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas.Any(u => u.CivilId == civilId && u.IssueId != issueId);

            if (AcceptedMemberExists)
            {
                return 2;
            }

            switch (civilId[0])
            {
                case '2':
                    yearPrefix = "19";
                    break;
                case '3':
                    yearPrefix = "20";
                    break;
                default:
                    return 1;
            }

            string year = yearPrefix + dobPart.Substring(0, 2);
            string month = dobPart.Substring(2, 2);
            string day = dobPart.Substring(4, 2);

            if (!DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                return 1;

            int[] weights = { 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;

            for (int i = 0; i < 11; i++)
            {
                sum += (civilId[i] - '0') * weights[i];
            }

            int checksum = 11 - (sum % 11);
            if (checksum == 11) checksum = 0;

            if (checksum == 10) return 1;

            return (civilId[11] - '0') == checksum ? 3 : 1;
        }

        public long IsValidatePassportNoforEdit(string PassportNo, long? issueId)
        {
            if (string.IsNullOrWhiteSpace(PassportNo)) return 1;
            if (PassportNo.Length != 8) return 1;

            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas.Any(u => u.PassportNo == PassportNo && u.IssueId != issueId);
            //   var MemberExists = _dbContext.MembershipRequestDetails.Any(u => u.PassportNo == PassportNo);
            if (AcceptedMemberExists)
            {
                return 2;
            }
            var pattern = @"^[A-Z][0-9]\d{5}[0-9]$";
            return Regex.IsMatch(PassportNo, pattern) ? 3 : 1;
        }

        public bool IsValidContactNoforEdit(long ContactNo, long? issueId)
        {
            var AcceptedMemberExists = _dbContext.MembershipAcceptedDatas.Any(u => u.ContactNo == ContactNo && u.IssueId != issueId);

            if (AcceptedMemberExists)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion MemeberShipValidations

        public ResponseEntity<List<PostMembershipViewModel>> GetMemberDetails(string? Keyword, string? SearchText)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {
                IQueryable<MembershipAccepted> query = _dbContext.MembershipAcceptedDatas.Include(x => x.WorkPlace)
                            .Include(x => x.Profession)
                            .Include(x => x.AreaData)
                            .Include(x => x.Zone)
                            .Include(x => x.Unit);

                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    switch (SearchText?.Trim().ToLower())
                    {
                        case "fullname":
                            query = query.Where(m => m.Name.Contains(Keyword));
                            break;

                        case "civilid":
                            query = query.Where(m => m.CivilId.Contains(Keyword));
                            break;

                        case "passport number":
                            query = query.Where(m => m.PassportNo.Contains(Keyword));
                            break;

                        case "contact number":
                            query = query.Where(m => m.ContactNo.ToString().Contains(Keyword));
                            break;

                        case "all":
                        default:
                            query = query.Where(m =>
                                m.Name.Contains(Keyword) ||
                                m.CivilId.Contains(Keyword) ||
                                m.PassportNo.Contains(Keyword) ||
                                m.ContactNo.ToString().Contains(Keyword));
                            break;
                    }
                }
                // Load list of members
                var members = query.ToList();

                // Now join manually with MemberProfileData and FileStorage
                var memberIds = members.Select(m => m.IssueId).ToList();

                var profilePhotos = _dbContext.MemberProfileDatas
                  .Where(p => memberIds.Contains(p.MemberId.Value))
                  .Include(p => p.FileStorage)
                  .AsEnumerable()
                  .DistinctBy(p => p.MemberId)
                  .ToDictionary(
                      p => p.MemberId,
                      p => p.FileStorage?.FilePath
                  );
                var departmentData = _dbContext.Departments.Where(x => x.Active);
                var objModel = query.Select(m => new PostMembershipViewModel
                {
                    // Assuming photo files are stored and you save relative paths in DB:


                    IssueId = m.IssueId,
                    Name = m.Name,
                    CivilId = m.CivilId,
                    ContactNo = m.ContactNo,
                    ReferenceNumber = m.ReferanceNo,
                    PaymentDone = (m.AmountRecieved ?? 0) == 0 ? false : true,
                    ProfileImagePath = profilePhotos.ContainsKey(m.IssueId) ? profilePhotos[m.IssueId] : "/images/Default.jpg",
                    Profession = m.Profession.ProffessionName,
                    WorkPlace = m.WorkPlace.WorkPlaceName,
                    Area = m.AreaData.AreaName,
                    Unit = m.Unit.UnitName,
                    DepartmentName = m.DepartmentId != null ? departmentData.FirstOrDefault(c => c.DepartmentId == m.DepartmentId).DepartmentName : null,
                }).ToList();

                // Assign to response entity
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

        public async Task<ResponseEntity<bool>> SaveAttachment(List<IFormFile> fileInputs, long? AttachmentMasterId, long? CreatedBy)
        {
            var objResponce = new ResponseEntity<bool>();

            if (fileInputs == null || !fileInputs.Any() || AttachmentMasterId == null)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                objResponce.returnMessage = "Invalid input parameters.";
                return objResponce;
            }

            try
            {
                List<MemberProfileData> attachmentsToSave = new List<MemberProfileData>();

                foreach (var f in fileInputs)
                {
                    var fileName = f.FileName;
                    if (fileName.LastIndexOf(@"\") > 0)
                        fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);

                    FileInfo fi = new FileInfo(fileName.ToLower());

                    if (!GenericUtilities.IsAllowedExtension(fi.Extension))
                        continue;

                    FileStorageViewModel objImage = new FileStorageViewModel();

                    var actualFileName = Path.GetFileNameWithoutExtension(f.FileName);

                    objImage.FileExtension = Path.GetExtension(f.FileName);
                    objImage.FileName = actualFileName;
                    objImage.ContentType = f.ContentType;
                    objImage.ContentLength = f.Length;
                    objImage.StorageMode = "LocalServer";

                    using (var memoryStream = new MemoryStream())
                    {
                        await f.CopyToAsync(memoryStream);
                        objImage.FileData = memoryStream.ToArray();
                    }

                    string folderName = DateTime.Now.ToString("yyyy/MM");
                    objImage.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", "MemberProfiles", folderName, AttachmentMasterId.ToString());

                    var attachResponse = await _attachmentRepository.SaveAttachment(objImage);

                    if (attachResponse.returnData != null)
                    {
                        attachmentsToSave.Add(new MemberProfileData
                        {
                            MemberId = AttachmentMasterId,
                            FileName = attachResponse.returnData.FileName,
                            OrgFileName = objImage.FileName,
                            FileStorageId = attachResponse.returnData.FileStorageId,
                            Active = true,
                            CreatedBy = CreatedBy != null ? CreatedBy : loggedInUser,
                            CreatedDate = DateTime.UtcNow
                        });
                    }
                }

                if (attachmentsToSave.Any())
                {
                    await _dbContext.MemberProfileDatas.AddRangeAsync(attachmentsToSave);
                    await _dbContext.SaveChangesAsync();

                    objResponce.returnData = true;
                    objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                    objResponce.returnMessage = "Files uploaded successfully.";
                }
                else
                {
                    objResponce.returnData = false;
                    objResponce.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    objResponce.returnMessage = "No valid files were uploaded.";
                }
            }
            catch (Exception ex)
            {
                objResponce.returnData = false;
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return objResponce;
        }

        public ResponseEntity<string> ExportMembersDatatoExcel(long? Area, long? Proffesionid, long? workplaceId, long? Genders, long? BloodGroups, long? Dist, long? Departments, long? WorkYr, long? Zones, long? Units, long? AgeGroup, long? PaidStatus, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var inputData = new MemberListFilter
                {
                    AreaId = Area,
                    ProffesionId = Proffesionid,
                    WorkplaceId = workplaceId,
                    Type = 0,
                    Gender = Genders,
                    BloodGroup = BloodGroups,
                    District = Dist,
                    Department = Departments,
                    Workingsince = WorkYr,
                    Zone = Zones,
                    Unit = Zones,
                    AgeGroupId = AgeGroup,
                    PaymentSatus = PaidStatus
                };

                var objData = GetAllAcceptedMembers(inputData);
                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();

                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("All Members List");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Membership ID";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Civil ID";
                        worksheet.Cell(1, 5).Value = "Contact No";
                        worksheet.Cell(1, 6).Value = "Email";
                        worksheet.Cell(1, 7).Value = "Profession";
                        worksheet.Cell(1, 8).Value = "Workplace";
                        worksheet.Cell(1, 9).Value = "Department";
                        worksheet.Cell(1, 10).Value = "Area";
                        worksheet.Cell(1, 11).Value = "Unit";
                        worksheet.Cell(1, 12).Value = "Payment Status";
                        worksheet.Cell(1, 13).Value = "Member since";
                        worksheet.Cell(1, 14).Value = "Last Membership Added";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(i + 2, 3).Value = item.Name;
                            worksheet.Cell(i + 2, 4).Value = item.CivilId;
                            worksheet.Cell(i + 2, 5).Value = item.PhoneNo;
                            worksheet.Cell(i + 2, 6).Value = item.Email;
                            worksheet.Cell(i + 2, 7).Value = item.Profession;
                            worksheet.Cell(i + 2, 8).Value = item.WorkPlace;
                            worksheet.Cell(i + 2, 9).Value = item.DepartmentName;
                            worksheet.Cell(i + 2, 10).Value = item.Area;
                            worksheet.Cell(i + 2, 11).Value = item.Unit;
                            if (objData.returnData[i].PaymentDone)
                            {
                                worksheet.Cell(i + 2, 12).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 12).Value = "Payment Pending";
                            }
                            worksheet.Cell(i + 2, 13).Value = item.Memberfrom?.ToString("dd-MM-yyyy");
                            worksheet.Cell(i + 2, 14).Value = item.LastMembershipAdded;
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

        public ResponseEntity<string> ExportRejectedMemberstoExcel(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllRejectedMembers(Area, 0, 0);


                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();

                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Rejected Members List");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Rejection ID";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Civil ID";
                        worksheet.Cell(1, 5).Value = "Contact No";
                        worksheet.Cell(1, 6).Value = "Email";
                        worksheet.Cell(1, 7).Value = "Profession";
                        worksheet.Cell(1, 8).Value = "Workplace";
                        worksheet.Cell(1, 9).Value = "Department";
                        worksheet.Cell(1, 10).Value = "Area";
                        worksheet.Cell(1, 11).Value = "Unit";
                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(i + 2, 3).Value = item.Name;
                            worksheet.Cell(i + 2, 4).Value = item.CivilId;
                            worksheet.Cell(i + 2, 5).Value = item.PhoneNo;
                            worksheet.Cell(i + 2, 6).Value = item.Email;
                            worksheet.Cell(i + 2, 7).Value = item.Profession;
                            worksheet.Cell(i + 2, 8).Value = item.WorkPlace;
                            worksheet.Cell(i + 2, 9).Value = item.DepartmentName;
                            worksheet.Cell(i + 2, 10).Value = item.Area;
                            worksheet.Cell(i + 2, 11).Value = item.Unit;
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

        public ResponseEntity<string> ExportRejectedMemberstoPdf(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllRejectedMembers(Area, 0, 0); // Adjust filters as needed

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();
                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate PDF using iTextSharp
                    using (var ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        FixITextSharpVersionBug(); // call this before GetInstance()
                        PdfWriter.GetInstance(document, ms);
                        document.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                        Paragraph title = new Paragraph("Rejected Members List", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 15f
                        };
                        document.Add(title);

                        PdfPTable table = new PdfPTable(11)
                        {
                            WidthPercentage = 100
                        };
                        table.SetWidths(new float[] { 3f, 6f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f });

                        // Add headers
                        string[] headers = { "Sl No", "Rejection ID", "Full Name", "Civil ID", "Phone", "Email", "Profession", "Workplace", "Department", "Area", "Unit" };
                        foreach (var header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                            table.AddCell(cell);
                        }

                        // Add rows
                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            table.AddCell(new Phrase((i + 1).ToString(), cellFont));
                            table.AddCell(new Phrase(item.ReferenceNumber ?? "", cellFont));
                            table.AddCell(new Phrase(item.Name ?? "", cellFont));
                            table.AddCell(new Phrase(item.CivilId ?? "", cellFont));
                            table.AddCell(new Phrase(item.PhoneNo ?? "", cellFont));
                            table.AddCell(new Phrase(item.Email ?? "", cellFont));
                            table.AddCell(new Phrase(item.Profession ?? "", cellFont));
                            table.AddCell(new Phrase(item.WorkPlace ?? "", cellFont));
                            table.AddCell(new Phrase(item.DepartmentName ?? "", cellFont));
                            table.AddCell(new Phrase(item.Area ?? "", cellFont));
                            table.AddCell(new Phrase(item.Unit ?? "", cellFont));
                        }

                        document.Add(table);
                        document.Close();

                        byte[] pdfBytes = ms.ToArray();

                        // Save or convert to Base64 or link etc. via your utility
                        retModel.returnData = GenericUtilities.SetReportData(pdfBytes, ".pdf");
                        retModel.transactionStatus = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Error generating PDF: " + ex.Message;
            }

            return retModel;
        }

        public static void FixITextSharpVersionBug()
        {
            var versionField = typeof(iTextSharp.text.Version).GetField("version", BindingFlags.Static | BindingFlags.NonPublic);
            if (versionField != null && versionField.GetValue(null) == null)
            {
                // Create a dummy version to avoid null reference
                versionField.SetValue(null, Activator.CreateInstance(typeof(iTextSharp.text.Version), true));
            }
        }

        public ResponseEntity<string> ExportAllMemberstoPdf(long? Area, long? Proffesionid, long? workplaceId, long? Genders, long? BloodGroups, long? Dist, long? Departments, long? WorkYr, long? Zones, long? Units, long? AgeGroup, long? PaidStatus, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();



            try
            {
                var inputData = new MemberListFilter
                {
                    AreaId = Area,
                    ProffesionId = Proffesionid,
                    WorkplaceId = workplaceId,
                    Type = 0,
                    Gender = Genders,
                    BloodGroup = BloodGroups,
                    District = Dist,
                    Department = Departments,
                    Workingsince = WorkYr,
                    Zone = Zones,
                    Unit = Zones,
                    AgeGroupId = AgeGroup,
                    PaymentSatus = PaidStatus
                };

                var objData = GetAllAcceptedMembers(inputData);

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword filter
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();
                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Create PDF Document
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);

                        FixITextSharpVersionBug(); // call this before GetInstance()
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // Add Title
                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        Paragraph title = new Paragraph("All Members List", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 20f
                        };
                        pdfDoc.Add(title);

                        // Create table with 8 columns
                        PdfPTable table = new PdfPTable(14);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 3f, 6f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 12f, 7f });

                        // Header row
                        string[] headers = { "Sl No", "Membership ID", "Full Name", "Civil ID", "Contact No", "Email", "Profession", "Workplace", "Department", "Area", "Unit", "Status", "Member since", "LastMembershipAdded" };
                        foreach (var header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                            table.AddCell(cell);
                        }

                        // Data rows
                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];

                            table.AddCell(new PdfPCell(new Phrase((i + 1).ToString())));
                            table.AddCell(new PdfPCell(new Phrase(item.ReferenceNumber ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.Name ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.CivilId ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.PhoneNo ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.Email ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.Profession ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.WorkPlace ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.DepartmentName ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.Area ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.Unit ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.PaymentDone ? "Active" : "Payment Pending")));
                            table.AddCell(new PdfPCell(new Phrase(item.Memberfrom?.ToString("dd-MM-yyyy") ?? "")));
                            table.AddCell(new PdfPCell(new Phrase(item.LastMembershipAdded ?? "")));
                        }

                        pdfDoc.Add(table);
                        pdfDoc.Close();

                        // Convert to byte[] and return
                        byte[] pdfBytes = stream.ToArray();
                        retModel.returnData = GenericUtilities.SetReportData(pdfBytes, ".pdf");
                        retModel.transactionStatus = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Error generating PDF: " + ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllMembersByArea(long? areaId, long? professionId, long? workplaceId, long? UnitId, long? DepartmentId, long? ZoneId, long? userId, string? Value)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {
                List<long> areaIds = new();

                if (areaId.HasValue)
                {

                    areaIds.Add(areaId.Value);
                }
                else
                {
                    // Get all zones for the user
                    areaIds = _dbContext.AreaMembers
                                .Where(z => z.UserMemberId == userId)
                                .Select(z => z.AreaId)
                                .ToList();
                }

                if (areaIds == null || !areaIds.Any())
                {
                    retModel.returnData = new List<PostMembershipViewModel>();
                    retModel.transactionStatus = HttpStatusCode.OK;
                    return retModel;
                }

                var query = _dbContext.MembershipAcceptedDatas
                    .Include(m => m.AreaData)
                    .Include(m => m.Profession)
                    .Include(m => m.WorkPlace)

                    .Where(m => m.Active && areaIds.Contains(m.AreaId ?? 0));

                // Apply filters
                //if (professionId.HasValue && professionId.Value > 0)
                //    query = query.Where(m => m.ProfessionId == professionId.Value);

                //if (workplaceId.HasValue && workplaceId.Value > 0)
                //    query = query.Where(m => m.WorkPlaceId == workplaceId.Value);
                //if (UnitId.HasValue && UnitId.Value > 0)
                //    query = query.Where(m => m.UnitId == UnitId.Value);
                //if (DepartmentId.HasValue && DepartmentId.Value > 0)
                //    query = query.Where(m => m.DepartmentId == DepartmentId.Value);
                //if (ZoneId.HasValue && ZoneId.Value > 0)
                //    query = query.Where(m => m.ZoneId == ZoneId.Value);
                if (!string.IsNullOrEmpty(Value))
                {
                    var status = Value.Trim().ToLower();
                    if (status == "paid")
                    {
                        query = query.Where(m => (m.AmountRecieved ?? 0) > 0);
                    }
                    else if (status == "unpaid")
                    {
                        query = query.Where(m => (m.AmountRecieved ?? 0) == 0);
                    }
                    //if (status == "paid")
                    //{
                    //    query = query.Where(m => m.AmountRecieved != 0);
                    //}
                    //else if (status == "unpaid")
                    //{
                    //    query = query.Where(m => m.AmountRecieved == 0);
                    //}
                    // else: treat as 'all' — no extra filter
                }

                retModel.returnData = query.Select(member => new PostMembershipViewModel
                {
                    IssueId = member.IssueId,
                    ReferenceNumber = member.ReferanceNo,
                    Name = member.Name,
                    CivilId = member.CivilId,
                    PhoneNo = member.ContactNo != null ? member.ContactNo.ToString() : "",
                    Email = member.Email,
                    Area = member.AreaData != null ? member.AreaData.AreaName : "",
                    //PaymentDone = member.AmountRecieved != 0,
                    PaymentDone = (member.AmountRecieved ?? 0) > 0,
                    ProfessionId = member.ProfessionId,
                    Profession = member.Profession != null ? member.Profession.ProffessionName : "",
                    WorkPlaceId = member.WorkPlaceId,
                    WorkPlace = member.WorkPlace != null ? member.WorkPlace.WorkPlaceName : "",
                    AmountRecieved = member.AmountRecieved,
                    //AmountInt = Convert.ToInt32(member.AmountRecieved),
                    AmountInt = Convert.ToInt32(member.AmountRecieved ?? 0),
                    Updates = member.Updates,
                    DepartmentId = member.DepartmentId,
                    DepartmentName = _dbContext.Departments
                           .Where(e => e.DepartmentId == member.DepartmentId)
                           .Select(e => e.DepartmentName)
                           .FirstOrDefault(),
                    UnitId = member.UnitId,
                    Unit = _dbContext.Units
                                .Where(e => e.UnitId == member.UnitId)
                                .Select(e => e.UnitName)
                                .FirstOrDefault(),
                    ZoneId = member.ZoneId,
                    Zone = _dbContext.Zones
                            .Where(e => e.ZoneId == member.ZoneId)
                               .Select(e => e.ZoneName)
                               .FirstOrDefault(),
                    SelectedRadio = Value

                }).ToList();

                retModel.transactionStatus = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
                retModel.returnData = null;
            }

            return retModel;

        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllMembersByUnit(long? unitId, long? Proffesionid, long? workplaceId, long? userId, string? Value)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {
                List<long> unitIds = new();

                if (unitId.HasValue)
                {

                    unitIds.Add(unitId.Value);
                }
                else
                {

                    unitIds = _dbContext.UnitMembers
                                .Where(z => z.UserMemberId == userId)
                                .Select(z => z.UnitId)
                                .ToList();
                }

                if (unitIds == null || !unitIds.Any())
                {
                    retModel.returnData = new List<PostMembershipViewModel>();
                    retModel.transactionStatus = HttpStatusCode.OK;
                    return retModel;
                }

                var query = _dbContext.MembershipAcceptedDatas
                    .Include(m => m.Unit)
                    .Include(m => m.Profession)
                    .Include(m => m.WorkPlace)
                    .Include(m => m.AreaData)
                    .Where(m => m.Active && unitIds.Contains(m.UnitId ?? 0));

                // Apply filters
                //if (Proffesionid.HasValue && Proffesionid.Value > 0)
                //    query = query.Where(m => m.ProfessionId == Proffesionid.Value);

                //if (workplaceId.HasValue && workplaceId.Value > 0)
                //    query = query.Where(m => m.WorkPlaceId == workplaceId.Value);
                if (!string.IsNullOrEmpty(Value))
                {
                    var status = Value.Trim().ToLower();
                    if (status == "paid")
                    {
                        query = query.Where(m => (m.AmountRecieved ?? 0) > 0);
                    }
                    else if (status == "unpaid")
                    {
                        query = query.Where(m => (m.AmountRecieved ?? 0) == 0);
                    }
                    //if (status == "paid")
                    //{
                    //    query = query.Where(m => m.AmountRecieved != 0);
                    //}
                    //else if (status == "unpaid")
                    //{
                    //    query = query.Where(m => m.AmountRecieved == 0);
                    //}
                    // else: treat as 'all' — no extra filter
                }

                retModel.returnData = query.Select(member => new PostMembershipViewModel
                {
                    IssueId = member.IssueId,
                    ReferenceNumber = member.ReferanceNo,
                    Name = member.Name,
                    CivilId = member.CivilId,
                    PhoneNo = member.ContactNo != null ? member.ContactNo.ToString() : "",
                    Email = member.Email,
                    Area = member.AreaData != null ? member.AreaData.AreaName : "",
                    //PaymentDone = member.AmountRecieved != 0,
                    PaymentDone = (member.AmountRecieved ?? 0) > 0,
                    ProfessionId = member.ProfessionId,
                    Profession = member.Profession != null ? member.Profession.ProffessionName : "",
                    WorkPlaceId = member.WorkPlaceId,
                    WorkPlace = member.WorkPlace != null ? member.WorkPlace.WorkPlaceName : "",
                    AmountRecieved = member.AmountRecieved,
                    //AmountInt = Convert.ToInt32(member.AmountRecieved),
                    AmountInt = Convert.ToInt32(member.AmountRecieved ?? 0),
                    Updates = member.Updates,
                    DepartmentId = member.DepartmentId,
                    DepartmentName = _dbContext.Departments
                           .Where(e => e.DepartmentId == member.DepartmentId)
                           .Select(e => e.DepartmentName)
                           .FirstOrDefault(),
                    UnitId = member.UnitId,
                    Unit = _dbContext.Units
                                .Where(e => e.UnitId == member.UnitId)
                                .Select(e => e.UnitName)
                                .FirstOrDefault(),
                    ZoneId = member.ZoneId,
                    Zone = _dbContext.Zones
                            .Where(e => e.ZoneId == member.ZoneId)
                               .Select(e => e.ZoneName)
                               .FirstOrDefault(),
                    SelectedRadio = Value
                }).ToList();

                retModel.transactionStatus = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
                retModel.returnData = null;
            }

            return retModel;

        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllMembersByZone(long? zoneId, long? Proffesionid, long? workplaceId, long? userId, string? Value)
        {
            var retModel = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {
                List<long> zoneIds = new();

                if (zoneId.HasValue)
                {
                    // Use the passed zone
                    zoneIds.Add(zoneId.Value);
                }
                else
                {
                    // Get all zones for the user
                    zoneIds = _dbContext.ZoneMembers
                                .Where(z => z.UserMemberId == userId)
                                .Select(z => z.ZoneId)
                                .ToList();
                }

                if (zoneIds == null || !zoneIds.Any())
                {
                    retModel.returnData = new List<PostMembershipViewModel>();
                    retModel.transactionStatus = HttpStatusCode.OK;
                    return retModel;
                }

                var query = _dbContext.MembershipAcceptedDatas
                    .Include(m => m.Zone)
                    .Include(m => m.Profession)
                    .Include(m => m.WorkPlace)
                    .Include(m => m.AreaData)
                    .Where(m => m.Active && zoneIds.Contains(m.ZoneId ?? 0));

                // Apply filters
                //if (Proffesionid.HasValue && Proffesionid.Value > 0)
                //    query = query.Where(m => m.ProfessionId == Proffesionid.Value);

                //if (workplaceId.HasValue && workplaceId.Value > 0)
                //    query = query.Where(m => m.WorkPlaceId == workplaceId.Value);
                if (!string.IsNullOrEmpty(Value))
                {
                    var status = Value.Trim().ToLower();

                    if (status == "paid")
                    {
                        query = query.Where(m => (m.AmountRecieved ?? 0) > 0);
                    }
                    else if (status == "unpaid")
                    {
                        query = query.Where(m => (m.AmountRecieved ?? 0) == 0);
                    }
                    // else: treat as 'all' — no extra filter
                }
                retModel.returnData = query.Select(member => new PostMembershipViewModel
                {
                    IssueId = member.IssueId,
                    ReferenceNumber = member.ReferanceNo,
                    Name = member.Name,
                    CivilId = member.CivilId,
                    PhoneNo = member.ContactNo != null ? member.ContactNo.ToString() : "",
                    Email = member.Email,
                    Area = member.AreaData != null ? member.AreaData.AreaName : "",
                    PaymentDone = (member.AmountRecieved ?? 0) > 0,
                    ProfessionId = member.ProfessionId,
                    Profession = member.Profession != null ? member.Profession.ProffessionName : "",
                    WorkPlaceId = member.WorkPlaceId,
                    WorkPlace = member.WorkPlace != null ? member.WorkPlace.WorkPlaceName : "",
                    AmountRecieved = member.AmountRecieved,
                    AmountInt = Convert.ToInt32(member.AmountRecieved ?? 0),
                    Updates = member.Updates,
                    DepartmentId = member.DepartmentId,
                    DepartmentName = _dbContext.Departments
                           .Where(e => e.DepartmentId == member.DepartmentId)
                           .Select(e => e.DepartmentName)
                           .FirstOrDefault(),
                    UnitId = member.UnitId,
                    Unit = _dbContext.Units
                                .Where(e => e.UnitId == member.UnitId)
                                .Select(e => e.UnitName)
                                .FirstOrDefault(),
                    ZoneId = member.ZoneId,
                    Zone = _dbContext.Zones
                            .Where(e => e.ZoneId == member.ZoneId)
                               .Select(e => e.ZoneName)
                               .FirstOrDefault(),
                    SelectedRadio = Value
                }).ToList();

                retModel.transactionStatus = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
                retModel.returnData = null;
            }

            return retModel;


        }

        public void UpdateOtpPreferences(long issueId, bool isMobileOtp, bool isEmailOtp)
        {
            var member = _dbContext.MembershipAcceptedDatas.FirstOrDefault(m => m.IssueId == issueId);
            if (member != null)
            {
                member.MobileOtp = isMobileOtp;
                member.EmailOtp = isEmailOtp;
                _dbContext.SaveChanges();
            }
        }

        public async Task<ResponseEntity<PostMembershipViewModel>> EditMembershipAsync(PostMembershipViewModel model)
        {
            var retModel = new ResponseEntity<PostMembershipViewModel>();

            try
            {

                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                // 1. Fetch the existing member by MembershipId
                var member = await _dbContext.MembershipAcceptedDatas
                    .FirstOrDefaultAsync(x => x.IssueId == model.IssueId);

                if (member == null)
                {
                    retModel.transactionStatus = HttpStatusCode.NotFound;
                    retModel.returnMessage = "Membership record not found.";
                    return retModel;
                }

                // 2. Check for duplicate Civil ID or Contact No (excluding current record)
                bool duplicateExists = await _dbContext.MembershipAcceptedDatas
                    .AnyAsync(x => x.IssueId != model.IssueId &&
                                  (x.CivilId == model.CivilId || x.ContactNo == model.ContactNo));

                if (duplicateExists)
                {
                    retModel.transactionStatus = HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Another member with the same Civil ID or Contact Number already exists.";
                    return retModel;
                }

                // 3. Update member fields
                member.Name = model.Name;
                member.CivilId = model.CivilId;
                member.PassportNo = model.PassportNo;
                member.DateofBirth = model.DateofBirth;
                member.GenderId = model.GenderId;
                member.BloodGroupId = model.BloodGroupId;
                member.ProfessionId = model.ProfessionId;
                member.CountryCodeId = model.CountryCodeId;
                member.ContactNo = model.ContactNo;
                member.WhatsAppNoCountryCodeid = model.WhatsAppNoCountryCodeid;
                member.WhatsAppNo = model.WhatsAppNo;
                member.Email = model.Email;
                member.AreaId = model.AreaId;
                member.ZoneId = model.ZoneId;
                member.UnitId = model.UnitId;
                member.Company = model.Company;
                member.KuwaitAddres = model.KuwaitAddress;

                member.PermenantAddress = model.PermenantAddress;
                member.Pincode = model.Pincode;

                member.EmergencyContactName = model.EmergencyContactName;
                member.EmergencyContactRelation = model.EmergencyContactRelation;
                member.EmergencyContactCountryCodeid = model.EmergencyContactCountryCodeid;
                member.EmergencyContactNumber = model.EmergencyContactNumber;
                member.EmergencyContactEmail = model.EmergencyContactEmail;

                member.MembershipNo = model.MembershipNo;

                member.UpdatedBy = model.loggedinUserId;
                member.UpdatedDate = DateTime.UtcNow;
                //member.WorkPlaceId = model.WorkPlaceId;
                //member.DistrictId = model.DistrictId;
                //member.HearAboutUsId = model.HearAboutUsId;
                ///member.DepartmentId = model.DepartmentId;
                //member.WorkYear = model.WorkYear;
                //member.ReferredBy = model.ReferredBy;



                if (model.FamilyData != null && model.FamilyData.Any())
                {
                    var ErrorMessage = new List<string>();
                    var AdultMembersList = new List<FamilyMembersData>();
                    var MinorMembersList = new List<FamilyMembersData>();
                    var MembershipAcceptedDataList = _dbContext.MembershipAcceptedDatas.Where(i => i.Active).ToList();
                    var memberShipRequestList = _dbContext.MembershipRequestDetails.Where(i => i.Active).ToList();

                    var ExistingMembers = _dbContext.MinorApplicantsAcceptedDatas.Where(i => i.Active && i.ParentId == member.IssueId).ToList();

                    var AddedmembersId = model.FamilyData.Select(i => i.MembershipId).ToList();

                    var ExistingMembers3 = ExistingMembers.Where(i => AddedmembersId.Contains(i.MembershipId)).ToList();

                    foreach (var item in model.FamilyData)
                    {
                        if (item.IsChanged || item.IsNew)
                        {
                            var GenderId = new long();
                            var DobData = GenerateDobFromCivilId(item.CivilId);
                            if (DobData.transactionStatus != HttpStatusCode.OK)
                            {
                                ErrorMessage.Add("CivilId Given For Member " + item.Name + "is Invalid");
                            }
                            else if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId || u.PassportNo == item.PassportNo)) || (memberShipRequestList.Any(u => u.CivilId == item.CivilId || u.PassportNo == item.PassportNo)))
                            {
                                if ((MembershipAcceptedDataList.Any(u => u.CivilId == item.CivilId)) || (memberShipRequestList.Any(u => u.CivilId == item.CivilId)))
                                {
                                    retModel.returnMessage = "Civil ID Given For the Family Member " + item.Name + " " + "Already Exists";
                                }
                                else if ((MembershipAcceptedDataList.Any(u => u.PassportNo == item.PassportNo)) || (memberShipRequestList.Any(u => u.ContactNo == item.MobileNoRelative)))
                                {
                                    retModel.returnMessage = "Passport No Given For Family Member " + item.Name + " " + "Already Exists";
                                }
                                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                                return retModel;
                            }
                            else
                            {
                                item.DateOfBirth = DobData.returnData;
                            }
                            var relationName = _dbContext.LookupMasters.FirstOrDefault(r => r.LookUpId == item.RelationType)?.LookUpName;

                            if (relationName != null)
                            {
                                GenderId = GetGenderId(relationName.ToLower()) ?? 0;
                            }

                            item.GenderId = GenderId;
                            item.ParentId = member.IssueId;

                            var MemberAge = DateTime.UtcNow.Year - item.DateOfBirth.Value.Year;

                            if (item.IsChanged && item.IsNew)
                            {
                                if (MemberAge > 18)
                                {
                                    AdultMembersList.Add(item);
                                }
                                else if (MemberAge < 18)
                                {
                                    MinorMembersList.Add(item);
                                }
                            }
                            else if (item.IsChanged && !item.IsNew)
                            {
                                var ExistingMember = ExistingMembers3.FirstOrDefault(i => i.MembershipId == item.MembershipId);
                                if (ExistingMember != null)
                                {
                                    ExistingMember.Name = item.Name;
                                    ExistingMember.CivilId = item.CivilId;
                                    ExistingMember.PassportNo = item.PassportNo;
                                    ExistingMember.DateofBirth = item.DateOfBirth;
                                    ExistingMember.RelationType = item.RelationType;
                                    ExistingMember.GenderId = item.GenderId;
                                    ExistingMember.BloodGroupId = item.BloodGroupid;
                                    ExistingMember.CountryCode = model.CountryCodeId;
                                    ExistingMember.ContactNo = model.ContactNo;
                                    ExistingMember.Email = model.Email;
                                    ExistingMember.AreaId = model.AreaId;
                                    ExistingMember.KuwaitAddres = model.KuwaitAddress;
                                    ExistingMember.PermenantAddress = model.PermenantAddress;
                                    ExistingMember.Pincode = model.Pincode;
                                    ExistingMember.ParentId = item.ParentId;
                                    ExistingMember.Active = true;
                                    ExistingMember.UpdatedDate = DateTime.UtcNow;
                                    ExistingMember.UpdatedBy = loggedInUser;
                                }
                                _dbContext.MinorApplicantsAcceptedDatas.Update(ExistingMember);
                                _dbContext.SaveChanges();
                            }
                        }

                    }

                    //if (AdultMembersList.Any())
                    //{
                    //    var MembersList = AdultMembersList.Select(f => new MembershipDetails
                    //    {
                    //        Name = f.Name,
                    //        CivilId = f.CivilId,
                    //        PassportNo = f.PassportNo,
                    //        DateofBirth = f.DateOfBirth,
                    //        GenderId = f.GenderId,
                    //        BloodGroupId = f.BloodGroupid,
                    //        ProffessionId = f.Professionid,
                    //        CountryCode = f.CountryCodeid,
                    //        ContactNo = f.MobileNoRelative,
                    //        Email = f.EmailRelative,
                    //        AreaId = model.AreaId,
                    //        Company = f.CompanyName,
                    //        KuwaitAddres = model.KuwaitAddress,
                    //        PermenantAddress = model.PermenantAddress,
                    //        Pincode = model.Pincode,
                    //        ParentId = f.ParentId,
                    //        Active = true,
                    //        CreatedDate = DateTime.UtcNow,
                    //        CreatedBy = loggedInUser,
                    //    }).ToList();

                    //    _dbContext.MembershipRequestDetails.AddRange(MembersList);
                    //    _dbContext.SaveChanges();
                    //}
                    //if (MinorMembersList.Any())
                    //{
                    //    var MembersList = MinorMembersList.Select(f => new MinorApplicantDetails
                    //    {
                    //        Name = f.Name,
                    //        CivilId = f.CivilId,
                    //        RelationType = f.RelationType,
                    //        PassportNo = f.PassportNo,
                    //        DateofBirth = f.DateOfBirth,
                    //        GenderId = f.GenderId,
                    //        BloodGroupId = f.BloodGroupid,
                    //        CountryCode = model.CountryCodeId,
                    //        ContactNo = model.ContactNo,
                    //        Email = model.Email,
                    //        AreaId = model.AreaId,
                    //        KuwaitAddres = model.KuwaitAddress,
                    //        PermenantAddress = model.PermenantAddress,
                    //        Pincode = model.Pincode,
                    //        ParentId = f.ParentId,
                    //        Active = true,
                    //        CreatedDate = DateTime.UtcNow,
                    //        CreatedBy = loggedInUser,
                    //    }).ToList();

                    //    _dbContext.MinorApplicantDetails.AddRange(MembersList);
                    //    _dbContext.SaveChanges();
                    //}
                }

                if (model.Attachment != null)
                {

                    // === 5. HARD DELETE old profile image ===
                    var oldAttachment = await _dbContext.MemberProfileDatas
                    .FirstOrDefaultAsync(x => x.MemberId == member.IssueId);

                    if (oldAttachment != null)
                    {
                        var fileStorage = await _dbContext.FileStorages
                            .FirstOrDefaultAsync(f => f.FileStorageId == oldAttachment.FileStorageId);

                        if (fileStorage != null)
                        {
                            string fullPath = Path.Combine(webRootPath, fileStorage.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                            if (File.Exists(fullPath))
                                File.Delete(fullPath);

                            _dbContext.FileStorages.Remove(fileStorage);
                        }

                        _dbContext.MemberProfileDatas.Remove(oldAttachment);
                    }

                    var DocUpload = await SaveAttachment(model.Attachment, model.IssueId, loggedInUser);
                }

                await _dbContext.SaveChangesAsync();

                // 5. Return success response
                retModel.returnData = model;
                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnMessage = "Membership updated successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An error occurred while updating the membership.";
            }

            return retModel;
        }

        public async Task<MembershipAccepted?> GetOtpSettingsRawAsync(long issueId)
        {
            return await _dbContext.MembershipAcceptedDatas
                .Where(x => x.IssueId == issueId)
                .FirstOrDefaultAsync();
        }

        public async Task<ResponseEntity<bool>> CancelMembershipAsync(long memberId, string? reason, string? description)
        {
            var response = new ResponseEntity<bool>();
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Find member in Accepted table
                var member = await _dbContext.MembershipAcceptedDatas
                    .FirstOrDefaultAsync(m => m.IssueId == memberId);

                var FamilyData = await _dbContext.MinorApplicantsAcceptedDatas.Where(f => f.ParentId == memberId && f.Active).ToListAsync();

                if (member == null)
                {
                    response.returnData = false;
                    response.transactionStatus = HttpStatusCode.NotFound;
                    response.returnMessage = "Member not found.";
                    return response;
                }

                // Step 2: Convert cancellation reason to long
                long? parsedReasonId = null;
                if (!string.IsNullOrWhiteSpace(reason) && long.TryParse(reason, out long tempReasonId))
                {
                    parsedReasonId = tempReasonId;
                }

                // Step 3: Map to cancelled entity
                var cancelledMember = new MemberShipCancelled
                {
                    ReferanceNo = member.ReferanceNo,
                    Name = member.Name,
                    CivilId = member.CivilId,
                    DateofBirth = member.DateofBirth,
                    PassportNo = member.PassportNo,
                    GenderId = member.GenderId,
                    BloodGroupId = member.BloodGroupId,
                    CountryCode = member.CountryCodeId,
                    ContactNo = member.ContactNo,
                    WhatsAppNoCountryCodeid = member.WhatsAppNoCountryCodeid,
                    WhatsAppNo = member.WhatsAppNo,
                    Email = member.Email,
                    ProfessionId = member.ProfessionId,
                    AreaId = member.AreaId,
                    Company = member.Company,
                    KuwaitAddres = member.KuwaitAddres,
                    PermenantAddress = member.PermenantAddress,
                    Pincode = member.Pincode,
                    EmergencyContactName = member.EmergencyContactName,
                    EmergencyContactRelation = member.EmergencyContactRelation,
                    EmergencyContactCountryCodeid = member.EmergencyContactCountryCodeid,
                    EmergencyContactNumber = member.EmergencyContactNumber,
                    EmergencyContactEmail = member.EmergencyContactEmail,

                    CampaignId = member.CampaignId,
                    CampaignAmount = member.CampaignAmount,
                    AmountRecieved = member.AmountRecieved,
                    PaymentTypeId = member.PaymentTypeId,
                    PaymentRemarks = member.PaymentRemarks,
                    HearAboutUsId = member.HearAboutUsId,
                    Memberfrom = member.Memberfrom,
                    MembershipRequestedDate = member.MembershipRequestedDate,
                    ApprovedBy = member.ApprovedBy,
                    ProffessionOther = member.ProffessionOther,
                    CancelReasonId = parsedReasonId,
                    CancellationRemarks = description,
                    CancelledBy = loggedInUser,
                    CreatedDate = DateTime.UtcNow,//CANCELLED DATE
                    PaidDate = member.PaidDate,
                    PaymentReceivedBy = member.PaymentReceivedBy,
                    EmailOtp = member.EmailOtp,
                    MobileOtp = member.MobileOtp,
                    WorkPlaceId = member.WorkPlaceId,
                    Active = true
                };

                await _dbContext.MemberShipCancelledDatas.AddAsync(cancelledMember);
                int insertResult = await _dbContext.SaveChangesAsync();

                if (FamilyData != null && FamilyData.Any())
                {
                    foreach (var familyMember in FamilyData)
                    {
                        var cancelledFamilyMember = new MinorApplicantsCancelledData
                        {
                            ParentId = cancelledMember.IssueId,
                            RelationType = familyMember.RelationType,
                            Name = familyMember.Name,
                            CivilId = familyMember.CivilId,
                            DateofBirth = familyMember.DateofBirth,
                            PassportNo = familyMember.PassportNo,
                            GenderId = familyMember.GenderId,
                            BloodGroupId = familyMember.BloodGroupId,
                            CountryCode = familyMember.CountryCode,
                            ContactNo = familyMember.ContactNo,
                            Email = familyMember.Email,
                            AreaId = familyMember.AreaId,
                            KuwaitAddres = familyMember.KuwaitAddres,
                            PermenantAddress = familyMember.PermenantAddress,
                            Pincode = familyMember.Pincode,
                            Active = true
                        };

                        await _dbContext.MinorApplicantsCancelledDatas.AddAsync(cancelledFamilyMember);
                        _dbContext.MinorApplicantsAcceptedDatas.Remove(familyMember);
                    }
                }

                if (insertResult > 0)
                {
                    var checkInserted = await _dbContext.MemberShipCancelledDatas
                        .FirstOrDefaultAsync(m =>
                            m.CivilId == member.CivilId &&
                            m.ContactNo == member.ContactNo &&
                            m.PassportNo == member.PassportNo);

                    if (checkInserted != null)
                    {
                        _dbContext.MembershipAcceptedDatas.Remove(member);
                        await _dbContext.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.returnData = true;
                        response.transactionStatus = HttpStatusCode.OK;
                        return response;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.returnData = false;
                        response.transactionStatus = HttpStatusCode.Conflict;
                        response.returnMessage = "Verification after insert failed.";
                        return response;
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    response.returnData = false;
                    response.transactionStatus = HttpStatusCode.InternalServerError;
                    response.returnMessage = "Insert failed.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.returnData = false;
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = ex.Message;
                return response;
            }
        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllActiveMembers(long? areaId, long? professionId, long? workplaceId)
        {
            var response = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {

                var activeCampaign = _dbContext.Campaigns.FirstOrDefault(c => c.Active);
                if (activeCampaign == null)
                {
                    response.transactionStatus = HttpStatusCode.OK;
                    response.returnData = new List<PostMembershipViewModel>();
                    return response;
                }

                var campaignStart = activeCampaign.StartDate ?? DateTime.MinValue;
                var campaignEnd = activeCampaign.EndDate ?? DateTime.Today;



                var departmentLookup = _dbContext.Departments
                  .ToDictionary(a => a.DepartmentId, a => a.DepartmentName);

                var memberQuery = _dbContext.MembershipAcceptedDatas
                    .Where(m => m.Active);

                if (areaId.HasValue && areaId.Value != 0)
                    memberQuery = memberQuery.Where(m => m.AreaId == areaId.Value);

                if (professionId.HasValue && professionId.Value != 0)
                    memberQuery = memberQuery.Where(m => m.ProfessionId == professionId.Value);

                if (workplaceId.HasValue && workplaceId.Value != 0)
                    memberQuery = memberQuery.Where(m => m.WorkPlaceId == workplaceId.Value);


                var paidMemberIds = _dbContext.MembershipFees
                    .Where(f =>
                        f.Campaign == activeCampaign.CampaignId &&
                        f.PaidAmount >= f.AmountToPay &&
                        f.Active &&
                        f.PaidDate.HasValue &&
                        f.PaidDate.Value.Date >= campaignStart.Date &&
                        f.PaidDate.Value.Date <= campaignEnd.Date)
                    .Select(f => f.MemberID)
                    .Distinct()
                    .ToList();


                var activeMembers = memberQuery
                    .Where(m => paidMemberIds.Contains(m.IssueId))
                    .Select(m => new PostMembershipViewModel
                    {
                        IssueId = m.IssueId,
                        ReferenceNumber = m.ReferanceNo,
                        Name = m.Name,
                        CivilId = m.CivilId,
                        PhoneNo = "+" + m.CountryCodeId + m.ContactNo,
                        Email = m.Email,
                        WorkPlace = m.WorkPlace != null ? m.WorkPlace.WorkPlaceName : "",
                        Profession = m.Profession != null ? m.Profession.ProffessionName : "",
                        Unit = m.Unit != null ? m.Unit.UnitName : "",
                        Area = m.AreaData != null ? m.AreaData.AreaName : "",
                        DepartmentName = departmentLookup.ContainsKey(m.DepartmentId ?? 0) ? departmentLookup[m.DepartmentId ?? 0] : "",
                    })
                    .OrderBy(m => m.Name)
                    .ToList();

                response.transactionStatus = HttpStatusCode.OK;
                response.returnData = activeMembers;
            }
            catch (Exception ex)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = ex.Message;
            }

            return response;
        }

        public ResponseEntity<string> ExportActiveMembersDatatoExcel(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllActiveMembers(Area, Proffesionid, workplaceId);


                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();

                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Active Members List");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Member ID";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Civil ID";
                        worksheet.Cell(1, 5).Value = "Contact No";
                        worksheet.Cell(1, 6).Value = "Email";
                        worksheet.Cell(1, 7).Value = "Profession";
                        worksheet.Cell(1, 8).Value = "Workplace";
                        worksheet.Cell(1, 9).Value = "Department";
                        worksheet.Cell(1, 10).Value = "Area";
                        worksheet.Cell(1, 11).Value = "Unit";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(i + 2, 3).Value = item.Name;
                            worksheet.Cell(i + 2, 4).Value = item.CivilId;
                            worksheet.Cell(i + 2, 5).Value = item.PhoneNo;
                            worksheet.Cell(i + 2, 6).Value = item.Email;
                            worksheet.Cell(i + 2, 7).Value = item.Profession;
                            worksheet.Cell(i + 2, 8).Value = item.WorkPlace;
                            worksheet.Cell(i + 2, 9).Value = item.DepartmentName;
                            worksheet.Cell(i + 2, 10).Value = item.Area;
                            worksheet.Cell(i + 2, 11).Value = item.Unit;
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

        public ResponseEntity<string> ExportActiveMemberstoPdf(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllActiveMembers(Area, Proffesionid, workplaceId);

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();
                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate PDF using iTextSharp
                    using (var ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        FixITextSharpVersionBug(); // call this before GetInstance()
                        PdfWriter.GetInstance(document, ms);
                        document.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                        Paragraph title = new Paragraph("Active Members List", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 15f
                        };
                        document.Add(title);

                        PdfPTable table = new PdfPTable(11)
                        {
                            WidthPercentage = 100
                        };
                        table.SetWidths(new float[] { 3f, 6f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f });


                        // Add headers
                        string[] headers = { "Sl No", "Membership ID", "Full Name", "Civil ID", "Contact No", "Email", "Profession", "Workplace", "Department", "Area", "Unit" };

                        foreach (var header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                            table.AddCell(cell);
                        }

                        // Add rows
                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            table.AddCell(new Phrase((i + 1).ToString(), cellFont));
                            table.AddCell(new Phrase(item.ReferenceNumber ?? "", cellFont));
                            table.AddCell(new Phrase(item.Name ?? "", cellFont));
                            table.AddCell(new Phrase(item.CivilId ?? "", cellFont));
                            table.AddCell(new Phrase(item.PhoneNo ?? "", cellFont));
                            table.AddCell(new Phrase(item.Email ?? "", cellFont));
                            table.AddCell(new Phrase(item.Profession ?? "", cellFont));
                            table.AddCell(new Phrase(item.WorkPlace ?? "", cellFont));
                            table.AddCell(new Phrase(item.DepartmentName ?? "", cellFont));
                            table.AddCell(new Phrase(item.Area ?? "", cellFont));
                            table.AddCell(new Phrase(item.Unit ?? "", cellFont));
                        }
                        document.Add(table);
                        document.Close();
                        byte[] pdfBytes = ms.ToArray();
                        // Save or convert to Base64 or link etc. via your utility
                        retModel.returnData = GenericUtilities.SetReportData(pdfBytes, ".pdf");
                        retModel.transactionStatus = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Error generating PDF: " + ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<List<PostMembershipViewModel>> GetAllExpiredMembers(long? areaId, long? professionId, long? workplaceId)
        {
            var response = new ResponseEntity<List<PostMembershipViewModel>>();

            try
            {

                var activeCampaign = _dbContext.Campaigns.FirstOrDefault(c => c.Active);
                if (activeCampaign == null)
                {
                    response.transactionStatus = HttpStatusCode.OK;
                    response.returnData = new List<PostMembershipViewModel>();
                    return response;
                }

                var campaignStart = activeCampaign.StartDate ?? DateTime.MinValue;
                var campaignEnd = activeCampaign.EndDate ?? DateTime.Today;


                var areaLookup = _dbContext.AreaDatas
                    .ToDictionary(a => a.AreaId, a => a.AreaName);

                var DepartmentLookup = _dbContext.Departments
                 .ToDictionary(a => a.DepartmentId, a => a.DepartmentName);
                var memberQuery = _dbContext.MembershipAcceptedDatas
                    .Where(m => m.Active);

                if (areaId.HasValue && areaId.Value != 0)
                    memberQuery = memberQuery.Where(m => m.AreaId == areaId.Value);

                if (professionId.HasValue && professionId.Value != 0)
                    memberQuery = memberQuery.Where(m => m.ProfessionId == professionId.Value);

                if (workplaceId.HasValue && workplaceId.Value != 0)
                    memberQuery = memberQuery.Where(m => m.WorkPlaceId == workplaceId.Value);


                var paidMemberIds = _dbContext.MembershipFees
                    .Where(f =>
                        f.Campaign == activeCampaign.CampaignId &&
                        f.PaidAmount >= f.AmountToPay &&
                        f.Active &&
                        f.PaidDate.HasValue &&
                        f.PaidDate.Value.Date >= campaignStart.Date &&
                        f.PaidDate.Value.Date <= campaignEnd.Date)
                    .Select(f => f.MemberID)
                    .Distinct()
                    .ToList();


                var expiredMembers = memberQuery
                    .Where(m => !paidMemberIds.Contains(m.IssueId))
                    .Select(m => new PostMembershipViewModel
                    {
                        IssueId = m.IssueId,
                        ReferenceNumber = m.ReferanceNo,
                        Name = m.Name,
                        CivilId = m.CivilId,
                        PhoneNo = "+" + m.CountryCodeId + m.ContactNo,
                        Email = m.Email,
                        WorkPlace = m.WorkPlace.WorkPlaceName,
                        Profession = m.Profession.ProffessionName,
                        Unit = m.Unit.UnitName,
                        DepartmentName = DepartmentLookup.ContainsKey(m.DepartmentId ?? 0) ? DepartmentLookup[m.DepartmentId ?? 0] : "",
                        Area = m.AreaData.AreaName
                        // Area = areaLookup.ContainsKey(m.AreaId ?? 0) ? areaLookup[m.AreaId ?? 0] : ""
                    })
                    .OrderBy(m => m.Name)
                    .ToList();

                response.transactionStatus = HttpStatusCode.OK;
                response.returnData = expiredMembers;
            }
            catch (Exception ex)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = ex.Message;
            }

            return response;
        }

        public ResponseEntity<string> ExportExpiredMembersDatatoExcel(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllExpiredMembers(Area, Proffesionid, workplaceId);


                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();

                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Expired Members List");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Member ID";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Civil ID";
                        worksheet.Cell(1, 5).Value = "Contact No";
                        worksheet.Cell(1, 6).Value = "Email";
                        worksheet.Cell(1, 7).Value = "Profession";
                        worksheet.Cell(1, 8).Value = "Workplace";
                        worksheet.Cell(1, 9).Value = "Department";
                        worksheet.Cell(1, 10).Value = "Area";
                        worksheet.Cell(1, 11).Value = "Unit";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(i + 2, 3).Value = item.Name;
                            worksheet.Cell(i + 2, 4).Value = item.CivilId;
                            worksheet.Cell(i + 2, 5).Value = item.PhoneNo;
                            worksheet.Cell(i + 2, 6).Value = item.Email;
                            worksheet.Cell(i + 2, 7).Value = item.Profession;
                            worksheet.Cell(i + 2, 8).Value = item.WorkPlace;
                            worksheet.Cell(i + 2, 9).Value = item.DepartmentName;
                            worksheet.Cell(i + 2, 10).Value = item.Area;
                            worksheet.Cell(i + 2, 11).Value = item.Unit;
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

        public ResponseEntity<string> ExportExpiredMemberstoPdf(long? Area, long? Proffesionid, long? workplaceId, string? searchField, string? keyword)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllExpiredMembers(Area, Proffesionid, workplaceId);

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    // Apply keyword-based filtering
                    if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        string lowerKeyword = keyword.ToLower().Trim();
                        objData.returnData = objData.returnData.Where(item =>
                            (searchField == "Name" && item.Name?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "CivilId" && item.CivilId?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "ReferenceNumber" && item.ReferenceNumber?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "Email" && item.Email?.ToLower().Contains(lowerKeyword) == true) ||
                            (searchField == "PhoneNo" && item.PhoneNo?.ToLower().Contains(lowerKeyword) == true)
                        ).ToList();
                    }

                    // Generate PDF using iTextSharp
                    using (var ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        FixITextSharpVersionBug(); // call this before GetInstance()
                        PdfWriter.GetInstance(document, ms);
                        document.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                        Paragraph title = new Paragraph("Unpaid Members List", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 15f
                        };
                        document.Add(title);

                        PdfPTable table = new PdfPTable(11)
                        {
                            WidthPercentage = 100
                        };

                        table.SetWidths(new float[] { 3f, 6f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f });

                        // Header row
                        string[] headers = { "Sl No", "Membership ID", "Full Name", "Civil ID", "Contact No", "Email", "Profession", "Workplace", "Department", "Area", "Unit" };

                        foreach (var header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                            table.AddCell(cell);
                        }

                        // Add rows
                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            var item = objData.returnData[i];
                            table.AddCell(new Phrase((i + 1).ToString(), cellFont));
                            table.AddCell(new Phrase(item.ReferenceNumber ?? "", cellFont));
                            table.AddCell(new Phrase(item.Name ?? "", cellFont));
                            table.AddCell(new Phrase(item.CivilId ?? "", cellFont));
                            table.AddCell(new Phrase(item.PhoneNo ?? "", cellFont));
                            table.AddCell(new Phrase(item.Email ?? "", cellFont));
                            table.AddCell(new Phrase(item.Profession ?? "", cellFont));
                            table.AddCell(new Phrase(item.WorkPlace ?? "", cellFont));
                            table.AddCell(new Phrase(item.DepartmentName ?? "", cellFont));
                            table.AddCell(new Phrase(item.Area ?? "", cellFont));
                            table.AddCell(new Phrase(item.Unit ?? "", cellFont));
                        }
                        document.Add(table);
                        document.Close();

                        byte[] pdfBytes = ms.ToArray();


                        retModel.returnData = GenericUtilities.SetReportData(pdfBytes, ".pdf");
                        retModel.transactionStatus = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Error generating PDF: " + ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<bool> ForceLogOutDevice(long? DeviceId)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var DeviceDetails = _dbContext.DeviceDetails.Find(DeviceId);
                retModel.returnData = false;
                if (DeviceDetails != null)
                {
                    var DeviceData = _dbContext.DeviceDetails.Where(i => i.DeviceId == DeviceDetails.DeviceId && !(i.IsForceLogout)).ToList();
                    foreach (var item in DeviceData)
                    {
                        item.Active = false;
                        item.IsForceLogout = true;
                        item.LogOutDateTime = DateTime.UtcNow;
                    }
                    _dbContext.SaveChanges();
                    retModel.returnData = true;
                    retModel.returnMessage = "Success";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        #region Methods for API
        public async Task<ResponseEntity<MemberDataViewModel>> GetMemberDataByCivilID(DeviceLoginViewModel InputData)
        {
            var Data = new ResponseEntity<MemberDataViewModel>();
            try
            {
                var RetData = new MemberDataViewModel();
                var TOKEN = "";
                var Objdata = _dbContext.MembershipAcceptedDatas.FirstOrDefault(i => i.CivilId == InputData.CivilIdInput);
                if (Objdata != null)
                {
                    var ForgedDeviceID = InputData.DeviceName + Objdata.IssueId;
                    var existingDevices = _dbContext.DeviceDetails.Where(i => i.DeviceId == ForgedDeviceID && i.CivilId == InputData.CivilIdInput).AsEnumerable();
                    if (existingDevices.Any())
                    {
                        await _dbContext.DeviceDetails
                        .Where(i => i.DeviceId == ForgedDeviceID && i.CivilId == InputData.CivilIdInput)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(d => d.IsUninstalled, true)
                            .SetProperty(d => d.Active, false)
                            .SetProperty(d => d.IsForceLogout, true)
                            .SetProperty(d => d.LogOutDateTime, DateTime.UtcNow)
                        );
                    }
                    var deviceData = new DeviceDetails()
                    {
                        DeviceId = ForgedDeviceID,
                        CivilId = InputData.CivilIdInput,
                        FCMToken = InputData.FCMToken,
                        DeviceName = InputData.DeviceName,
                        DeviceModel = InputData.DeviceModel,
                        DeviceType = InputData.DeviceType,
                        CreatedDate = DateTime.UtcNow,
                    };
                    await _dbContext.DeviceDetails.AddAsync(deviceData);
                    _dbContext.SaveChanges();

                    var DeviceUniqueID = deviceData.DeviceDetailId != null ? Convert.ToString(deviceData.DeviceDetailId) : null;
                    var Token = (InputData.DeviceId != null && InputData.CivilIdInput != null && DeviceUniqueID != null) ? await _authenticationServiceRepository.GenerateToken(DeviceUniqueID, InputData.DeviceId, InputData.CivilIdInput) : null;

                    RetData.CivilId = Objdata.CivilId;
                    RetData.Name = Objdata.Name;
                    RetData.MembershipId = Objdata.ReferanceNo;
                    RetData.PhoneNumber = Objdata.ContactNo;
                    RetData.OTPRequired = false;
                    RetData.IsBlackListed = false;
                    RetData.IsBlocked = false;
                    RetData.Token = Token;
                    RetData.IsRegistered = false;

                    Data.returnData = RetData;
                    Data.returnMessage = "success";
                    Data.transactionStatus = HttpStatusCode.OK;
                }
                else
                {
                    Data.transactionStatus = HttpStatusCode.BadRequest;
                    Data.returnMessage = "Member With CivilId Not Found, Make Sure Member Verification Is Completed";
                }
            }
            catch (Exception ex)
            {
                Data.transactionStatus = HttpStatusCode.InternalServerError;
                Data.returnMessage = ex.Message;
            }
            return Data;

        }

        public async Task<ResponseEntity<bool>> UploadProfilePicture(IFormFile Input, string CivilID, string DeviceId, long devicePrimaryID)
        {
            var retrunData = new ResponseEntity<bool>();
            loggedInUser = 1;
            try
            {
                var MemberData = await _dbContext.MembershipAcceptedDatas.FirstOrDefaultAsync(i => i.CivilId == CivilID);
                if (MemberData != null)
                {
                    var DeviceData = _dbContext.DeviceDetails.FirstOrDefault(i => i.DeviceDetailId == devicePrimaryID);
                    if (DeviceData != null)
                    {
                        var DocUpload = await SaveImageFromRegistartion(Input, DeviceData.DeviceDetailId, loggedInUser);
                        if (DocUpload.transactionStatus == HttpStatusCode.OK)
                        {
                            DeviceData.Active = true;
                            DeviceData.OrgFileName = DocUpload.returnData.OrgFileName;
                            DeviceData.FilePath = DocUpload.returnData.FilePath;
                            DeviceData.FileStorageId = DocUpload.returnData.FileStorageId;
                            DeviceData.UpdatedDate = DateTime.UtcNow;
                            DeviceData.LastOpenDateTime = DateTime.UtcNow;

                            await _dbContext.SaveChangesAsync();

                            retrunData.returnData = true;
                            retrunData.returnMessage = "success";
                            retrunData.transactionStatus = HttpStatusCode.OK;
                        }
                        else
                        {
                            retrunData.transactionStatus = HttpStatusCode.InternalServerError;
                            retrunData.returnMessage = "Some Error Occurred, Profile Image Could not be updated";
                        }
                    }
                    else
                    {
                        retrunData.transactionStatus = HttpStatusCode.InternalServerError;
                        retrunData.returnMessage = "Some Error Occurred, Device Could not be found";
                    }
                }
                else
                {
                    retrunData.transactionStatus = HttpStatusCode.BadRequest;
                    retrunData.returnMessage = "Member With CivilId Not Found, Make Sure Member Verification Is Completed";
                }
            }
            catch (Exception ex)
            {
                retrunData.transactionStatus = HttpStatusCode.InternalServerError;
                retrunData.returnMessage = ex.Message;
            }
            return retrunData;
        }

        public async Task<ResponseEntity<FileStorage>> SaveImageFromRegistartion(IFormFile fileInputs, long? AttachmentMasterId, long? CreatedBy)
        {
            var objResponce = new ResponseEntity<FileStorage>();

            if (fileInputs == null || AttachmentMasterId == null)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                objResponce.returnMessage = "Invalid input parameters.";
                return objResponce;
            }

            try
            {
                if (fileInputs != null)
                {
                    var fileName = fileInputs.FileName;
                    if (fileName.LastIndexOf(@"\") > 0)
                        fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);

                    FileInfo fi = new FileInfo(fileName.ToLower());

                    if (GenericUtilities.IsAllowedExtension(fi.Extension))
                    {
                        FileStorageViewModel objImage = new FileStorageViewModel();

                        var actualFileName = Path.GetFileNameWithoutExtension(fileInputs.FileName);

                        objImage.FileExtension = Path.GetExtension(fileInputs.FileName);
                        objImage.FileName = actualFileName;
                        objImage.ContentType = fileInputs.ContentType;
                        objImage.ContentLength = fileInputs.Length;
                        objImage.StorageMode = "LocalServer";

                        using (var memoryStream = new MemoryStream())
                        {
                            await fileInputs.CopyToAsync(memoryStream);
                            objImage.FileData = memoryStream.ToArray();
                        }

                        string folderName = DateTime.Now.ToString("yyyy/MM");
                        objImage.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", "DeviceProfiles", folderName, AttachmentMasterId.ToString());

                        var attachResponse = await _attachmentRepository.SaveAttachment(objImage);

                        if (attachResponse.returnData != null)
                        {
                            attachResponse.returnData.OrgFileName = objImage.FileName;
                            objResponce.returnData = attachResponse.returnData;
                            objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return objResponce;
        }

        public async Task<ResponseEntity<MemberDetailsData>> GetMemberDataPostLogin(string CivilID, string DeviceId, long devicePrimaryID)
        {
            var returnData = new ResponseEntity<MemberDetailsData>();
            try
            {
                var MemberData = new MemberDetailsData();
                var memberDetails = _dbContext.MembershipAcceptedDatas
                   .Include(c => c.Profession)
                   .Include(c => c.WorkPlace)
                   .Include(c => c.Unit)
                   .Include(c => c.Zone)
                   .Include(c => c.AreaData)
                   .Include(c => c.Campaign)
                   .FirstOrDefault(u => u.CivilId == CivilID);
                var deviceData = _dbContext.DeviceDetails.FirstOrDefault(i => i.DeviceDetailId == devicePrimaryID);
                if (memberDetails != null && deviceData != null)
                {
                    MemberData.Imagepath = deviceData.FilePath;
                    MemberData.Membername = memberDetails.Name;
                    MemberData.PaymentStatus = memberDetails.AmountRecieved != 0 && memberDetails.ApprovedBy != null ? true : false;
                    MemberData.MembershipNo = memberDetails.MembershipNo != null ? memberDetails.MembershipNo : memberDetails.ReferanceNo;
                    MemberData.CivilId = memberDetails.CivilId;
                    MemberData.Area = memberDetails.AreaId != null ?  memberDetails.AreaData.AreaName : null;
                    MemberData.Zone = memberDetails.ZoneId != null ? memberDetails?.Zone.ZoneName : "";
                    MemberData.Unit = memberDetails.UnitId != null ? memberDetails?.Unit.UnitName : "";
                    MemberData.Profession = memberDetails.ProfessionId != null ?  memberDetails?.Profession.ProffessionName : "";
                    //MemberData.WorkPlace = memberDetails.WorkPlaceId != null ? memberDetails?.WorkPlace.WorkPlaceName : null ;
                    MemberData.MobileNo = memberDetails.ContactNo != null ? memberDetails.ContactNo.ToString() : null;
                    MemberData.Email = memberDetails?.Email ?? "";
                    MemberData.BloodGroup = memberDetails.BloodGroupId != null ? _dbContext.LookupMasters.FirstOrDefault(i => i.LookUpId == memberDetails.BloodGroupId)?.LookUpName : null;
                    returnData.returnData = MemberData;
                    returnData.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    returnData.returnMessage = "Member Not Found";
                }
            }
            catch (Exception ex)
            {
                returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                returnData.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return returnData;
        }

        public async Task<ResponseEntity<PostMembershipViewModel>> UpdateRemark(long? Issueid, string? Remark)
        {
            var response = new ResponseEntity<PostMembershipViewModel>();

            if (Issueid == null)
            {
                response.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                response.returnMessage = "Invalid ID";
                return response;
            }

            // Example: Save to DB using EF Core
            var entity = await _dbContext.MembershipAcceptedDatas.FirstOrDefaultAsync(x => x.IssueId == Issueid);
            if (entity != null)
            {
                entity.Updates = Remark;
                await _dbContext.SaveChangesAsync();

                response.transactionStatus = System.Net.HttpStatusCode.OK;
                response.returnData = new PostMembershipViewModel { IssueId = Issueid.Value, PaymentRemarks = Remark };
            }
            else
            {
                response.transactionStatus = System.Net.HttpStatusCode.NotFound;
                response.returnMessage = "Record not found";
            }

            return response;
        }

        #endregion
        public async Task<ResponseEntity<MemberData>> GetMemberStatus(string CivilId)
        {
            var returnData = new ResponseEntity<MemberData>();
            var memberdata = new MemberData();
            try
            {
                var Memberdata = _dbContext.MembershipAcceptedDatas.SingleOrDefault(i => i.CivilId.Trim() == CivilId.Trim());
                if (Memberdata != null)
                {
                    memberdata.Name = Memberdata.Name;
                    memberdata.Count = Memberdata.AmountRecieved != 0 ? 1 : 0;
                    returnData.returnData = memberdata;
                    returnData.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                returnData.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return returnData;
        }

        public ResponseEntity<string> ExportMemberDatatoExcel(string search, long? area, long? unit, long? zone)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                // 🔹 Build filter
                var filter = new MemberListFilter
                {
                    AreaId = area,
                    Unit = unit,
                    Zone = zone,
                    SearchString = search,
                    SearchColumn = "Name", // 👈 adjust if you want to search by name
                    Pagesize = null // we want all data
                };

                // 🔹 Get filtered members
                var objData = GetAllAcceptedMembers(filter);

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Member List");

                        // 🔹 Header Row
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Member Id";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Proffession";
                        worksheet.Cell(1, 5).Value = "Work Place";
                        worksheet.Cell(1, 6).Value = "Department";
                        worksheet.Cell(1, 7).Value = "Area";
                        worksheet.Cell(1, 8).Value = "Unit";
                        worksheet.Cell(1, 9).Value = "Member-from";
                        worksheet.Cell(1, 10).Value = "Payed Till";
                        worksheet.Cell(1, 11).Value = "Status";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        // 🔹 Fill Data
                        var list = objData.returnData;
                        for (int i = 0; i < list.Count; i++)
                        {
                            var item = list[i];
                            int row = i + 2;

                            worksheet.Cell(row, 1).Value = i + 1;
                            worksheet.Cell(row, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(row, 3).Value = item.Name;
                            worksheet.Cell(row, 4).Value = item.Profession;
                            worksheet.Cell(row, 5).Value = item.WorkPlace;
                            worksheet.Cell(row, 6).Value = item.DepartmentName;
                            worksheet.Cell(row, 7).Value = item.Area;
                            worksheet.Cell(row, 8).Value = item.Unit;
                            worksheet.Cell(row, 9).Value = item.MemberfromString;
                            worksheet.Cell(row, 10).Value = item.CampaignEndDateString;
                            if (objData.returnData[i].PaymentDone)
                            {
                                worksheet.Cell(i + 2, 11).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 11).Value = "Inactive";
                            }
                        }

                        worksheet.Columns().AdjustToContents();

                        // 🔹 Export to byte[]
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
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<string> ExportMembershipDatatoExcel(string search, long? campaign)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                // 🔹 Build filter
                var filter = new MemberListFilter
                {
                    CampaignId = campaign,
                    SearchString = search,
                    SearchColumn = "Name", // 👈 adjust if you want to search by name
                    Pagesize = null // we want all data
                };

                // 🔹 Get filtered members
                var objData = GetAllAcceptedMembers(filter);

                if (objData.transactionStatus == HttpStatusCode.OK && objData.returnData != null)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Member List");

                        // 🔹 Header Row
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Member Id";
                        worksheet.Cell(1, 3).Value = "Full Name";
                        worksheet.Cell(1, 4).Value = "Proffession";
                        worksheet.Cell(1, 5).Value = "Work Place";
                        worksheet.Cell(1, 6).Value = "Department";
                        worksheet.Cell(1, 7).Value = "Area";
                        worksheet.Cell(1, 8).Value = "Unit";
                        worksheet.Cell(1, 9).Value = "Member-Since";
                        worksheet.Cell(1, 10).Value = "Payed Till";
                        worksheet.Cell(1, 11).Value = "Status";
                        worksheet.Cell(1, 12).Value = "Member since";
                        worksheet.Cell(1, 13).Value = "Last Membership Added";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        // 🔹 Fill Data
                        var list = objData.returnData;
                        for (int i = 0; i < list.Count; i++)
                        {
                            var item = list[i];
                            int row = i + 2;

                            worksheet.Cell(row, 1).Value = i + 1;
                            worksheet.Cell(row, 2).Value = item.ReferenceNumber;
                            worksheet.Cell(row, 3).Value = item.Name;
                            worksheet.Cell(row, 4).Value = item.Profession;
                            worksheet.Cell(row, 5).Value = item.WorkPlace;
                            worksheet.Cell(row, 6).Value = item.DepartmentName;
                            worksheet.Cell(row, 7).Value = item.Area;
                            worksheet.Cell(row, 8).Value = item.Unit;
                            worksheet.Cell(row, 9).Value = item.MemberfromString;
                            worksheet.Cell(row, 10).Value = item.CampaignEndDateString;
                            if (objData.returnData[i].PaymentDone)
                            {
                                worksheet.Cell(i + 2, 11).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 11).Value = "Payment Pending";
                            }
                            worksheet.Cell(i + 2, 12).Value = item.Memberfrom?.ToString("dd-MM-yyyy");
                            worksheet.Cell(i + 2, 13).Value = item.LastMembershipAdded;
                        }

                        worksheet.Columns().AdjustToContents();

                        // 🔹 Export to byte[]
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
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public async Task<ResponseEntity<MembershipViewModel>> UpdateRequestedMemberCivilid(MembershipViewModel model)
        {
            var retModel = new ResponseEntity<MembershipViewModel>();
            try
            {
                var memberData = _dbContext.MembershipRequestDetails.FirstOrDefault(i => i.CivilId == model.CivilId);


                memberData.Name = model.Name;
                memberData.CivilId = model.CivilId;
                memberData.PassportNo = model.PassportNo;
                memberData.DateofBirth = model.DOB;
                memberData.GenderId = model.Genderid;
                memberData.BloodGroupId = model.BloodGroupid;
                memberData.ProffessionId = model.Professionid;
                memberData.WorkPlaceId = model.WorkPlaceid;
                memberData.ContactNo = model.ContactNo;
                memberData.Email = model.Email;
                memberData.DistrictId = model.Districtid;
                memberData.AreaId = model.Areaid;
                memberData.CountryCode = model.CountryCodeid;
                memberData.UpdatedDate = DateTime.UtcNow;
                memberData.UpdatedBy = loggedInUser;
                memberData.HearAboutus = model.Hearaboutusid;
                memberData.DepartmentId = model.DepartmentId;
                memberData.WorkYear = model.WorkYear;

                await _dbContext.SaveChangesAsync();
                retModel.returnData = model;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Updated Successfully";

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public async Task<ResponseEntity<MembershipViewModel>> UpdateRequestedMemberPassport(MembershipViewModel model)
        {
            var retModel = new ResponseEntity<MembershipViewModel>();
            try
            {
                var memberData = _dbContext.MembershipRequestDetails.FirstOrDefault(i => i.PassportNo == model.PassportNo);


                memberData.Name = model.Name;
                memberData.CivilId = model.CivilId;
                memberData.PassportNo = model.PassportNo;
                memberData.DateofBirth = model.DOB;
                memberData.GenderId = model.Genderid;
                memberData.BloodGroupId = model.BloodGroupid;
                memberData.ProffessionId = model.Professionid;
                memberData.WorkPlaceId = model.WorkPlaceid;
                memberData.ContactNo = model.ContactNo;
                memberData.Email = model.Email;
                memberData.DistrictId = model.Districtid;
                memberData.AreaId = model.Areaid;
                memberData.CountryCode = model.CountryCodeid;
                memberData.UpdatedDate = DateTime.UtcNow;
                memberData.UpdatedBy = loggedInUser;
                memberData.HearAboutus = model.Hearaboutusid;
                memberData.DepartmentId = model.DepartmentId;
                memberData.WorkYear = model.WorkYear;

                await _dbContext.SaveChangesAsync();
                retModel.returnData = model;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Updated Successfully";

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public async Task<ResponseEntity<MembershipViewModel>> UpdateRequestedMemberContact(MembershipViewModel model)
        {
            var retModel = new ResponseEntity<MembershipViewModel>();
            try
            {
                var memberData = _dbContext.MembershipRequestDetails.FirstOrDefault(i => i.ContactNo == model.ContactNo
                );


                memberData.Name = model.Name;
                memberData.CivilId = model.CivilId;
                memberData.PassportNo = model.PassportNo;
                memberData.DateofBirth = model.DOB;
                memberData.GenderId = model.Genderid;
                memberData.BloodGroupId = model.BloodGroupid;
                memberData.ProffessionId = model.Professionid;
                memberData.WorkPlaceId = model.WorkPlaceid;
                memberData.ContactNo = model.ContactNo;
                memberData.Email = model.Email;
                memberData.DistrictId = model.Districtid;
                memberData.AreaId = model.Areaid;
                memberData.CountryCode = model.CountryCodeid;
                memberData.UpdatedDate = DateTime.UtcNow;
                memberData.UpdatedBy = loggedInUser;
                memberData.HearAboutus = model.Hearaboutusid;
                memberData.DepartmentId = model.DepartmentId;
                memberData.WorkYear = model.WorkYear;

                await _dbContext.SaveChangesAsync();
                retModel.returnData = model;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Updated Successfully";

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public ResponseEntity<DateTime> GenerateDobFromCivilId(string civilId)
        {
            var returnData = new ResponseEntity<DateTime>();

            try
            {
                if (string.IsNullOrEmpty(civilId) || !Regex.IsMatch(civilId, @"^[1-3]\d{11}$"))
                {
                    returnData.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    returnData.returnMessage = "Invalid Civil ID format.";
                    return returnData;
                }

                string dobPart = civilId.Substring(1, 6); // YYMMDD
                string yearPrefix;

                switch (civilId[0])
                {
                    case '1': // just in case older IDs start with 1
                    case '2':
                        yearPrefix = "19";
                        break;
                    case '3':
                        yearPrefix = "20";
                        break;
                    default:
                        returnData.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        returnData.returnMessage = "Invalid Civil ID prefix.";
                        return returnData;
                }

                string year = yearPrefix + dobPart.Substring(0, 2);
                string month = dobPart.Substring(2, 2);
                string day = dobPart.Substring(4, 2);

                if (int.TryParse(year, out int yr) &&
                    int.TryParse(month, out int mm) &&
                    int.TryParse(day, out int dd))
                {
                    if (mm >= 1 && mm <= 12 && dd >= 1 && dd <= DateTime.DaysInMonth(yr, mm))
                    {
                        var dob = new DateTime(yr, mm, dd);

                        returnData.returnData = dob;
                        returnData.transactionStatus = System.Net.HttpStatusCode.OK;
                        returnData.returnMessage = "DOB generated successfully.";
                    }
                    else
                    {
                        returnData.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        returnData.returnMessage = "Invalid date extracted from Civil ID.";
                    }
                }
                else
                {
                    returnData.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    returnData.returnMessage = "Unable to parse date components.";
                }
            }
            catch (Exception ex)
            {
                // log ex if needed
                returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                returnData.returnMessage = "Server Error";
            }

            return returnData;
        }

        public long? GetGenderId(string RelationType)
        {
            var GenderId = new long();
            try
            {
                var GenderTypes = _dbContext.LookupMasters.Where(i => i.LookUpTypeId == 4 && i.Active).ToList();
                if (GenderTypes.Any())
                {
                    var maleTypes = new List<string> { "father", "husband", "brother", "son", "uncle", "grandfather" };
                    var femaleTypes = new List<string> { "mother", "wife", "sister", "daughter", "aunt", "grandmother" };
                    if (maleTypes.Contains(RelationType))
                    {
                        GenderId = GenderTypes.FirstOrDefault(i => i.LookUpName.ToLower() == "male").LookUpId;
                    }
                    else if (femaleTypes.Contains(RelationType))
                    {
                        GenderId = GenderTypes.FirstOrDefault(i => i.LookUpName.Trim().ToLower() == "female").LookUpId;
                    }
                }
            }
            catch (Exception)
            {

            }
            return GenderId;
        }
    }
}