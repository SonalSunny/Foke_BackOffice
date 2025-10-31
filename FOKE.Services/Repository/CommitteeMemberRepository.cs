using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.CommitteeManagement.DTO;
using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class CommitteeMemberRepository : ICommitteeMemberRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        private readonly IAttachmentRepository _attachmentRepository;
        public CommitteeMemberRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachmentRepository)
        {
            this._dbContext = FOKEDBContext;
            _httpContextAccessor = httpContextAccessor;
            this._attachmentRepository = attachmentRepository;
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
            _attachmentRepository = attachmentRepository;
        }

        public async Task<ResponseEntity<CommitteMemberViewModel>> AddCommitteeMember(CommitteMemberViewModel model)
        {
            var retModel = new ResponseEntity<CommitteMemberViewModel>();

            try
            {
                //var exists = _dbContext.CommitteMembers
                //    .Any(m => m.Name.Trim().ToLower() == model.Name.Trim().ToLower()
                //           && m.GroupId == model.GroupId);

                //if (exists)
                //{
                //    retModel.transactionStatus = HttpStatusCode.Conflict;
                //    retModel.returnMessage = "Committee member already exists in this group.";
                //    return retModel;
                //}

                if (model.Photo != null)
                {
                    var fileResult = await SaveSingleFile(model.Photo, null, loggedInUser);
                    if (fileResult.transactionStatus == HttpStatusCode.OK)
                    {
                        model.ImagePath = fileResult.returnData.FilePath;
                        model.FileStorageId = fileResult.returnData.FileStorageId;
                    }
                    else
                    {
                        retModel.transactionStatus = HttpStatusCode.InternalServerError;
                        retModel.returnMessage = "Photo upload failed.";
                        return retModel;
                    }
                }

                var entity = new CommitteMember
                {
                    IssueId = model.IssueId,
                    GroupId = model.GroupId,
                    Name = model.Name,
                    Position = model.Position,
                    CountryCodeId = model.CountryCodeId,
                    ContactNo = model.ContactNo,
                    SortOrder = model.SortOrder,
                    ImagePath = model.ImagePath,
                    FileStorageId = model.FileStorageId,
                    CreatedBy = loggedInUser,
                    CreatedDate = DateTime.UtcNow,
                    Active = true,
                    AnyValue = model.AnyValue
                };

                await _dbContext.CommitteMembers.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnMessage = "Committee member added successfully.";
                retModel.returnData = model;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server error: " + ex.Message;
            }

            return retModel;
        }

        public async Task<ResponseEntity<FileStorage>> SaveSingleFile(IFormFile fileInputs, string? AttachmentMasterId, long? CreatedBy)
        {
            var objResponce = new ResponseEntity<FileStorage>();

            if (fileInputs == null)
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

                        string folderName = DateTime.UtcNow.ToString("yyyy/MM");
                        objImage.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", "CommitteeMemberImages", folderName);

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

        public ResponseEntity<List<CommitteMemberViewModel>> GetAllCommitteeMembers(long? status, long? committeeSearch, long? groupSearch)
        {
            var retModel = new ResponseEntity<List<CommitteMemberViewModel>>();
            try
            {
                var membersQuery = _dbContext.CommitteMembers
                    .Include(m => m.CommitteGroup)
                        .ThenInclude(g => g.Committee)
                    .Include(m => m.FileStorage)
                    .AsQueryable();

                if (status.HasValue)
                {
                    membersQuery = status.Value switch
                    {
                        0 => membersQuery.Where(m => !m.Active),
                        1 => membersQuery.Where(m => m.Active),
                        _ => membersQuery
                    };
                }

                if (groupSearch.HasValue)
                    membersQuery = membersQuery.Where(m => m.GroupId == groupSearch);

                if (committeeSearch.HasValue)
                    membersQuery = membersQuery.Where(m =>
                        m.CommitteGroup != null &&
                        m.CommitteGroup.CommitteeId == committeeSearch.Value);

                var result = (from member in membersQuery
                              join accepted in _dbContext.MembershipAcceptedDatas
                                  on member.IssueId equals accepted.IssueId into membGroup
                              from accepted in membGroup.DefaultIfEmpty()
                              select new CommitteMemberViewModel
                              {
                                  CommitteMemberId = member.CommitteMemberId,
                                  IssueId = member.IssueId,
                                  GroupId = member.GroupId,
                                  ImagePath = member.ImagePath,
                                  FileStorageId = member.FileStorageId,
                                  Name = member.Name,
                                  Position = member.Position,
                                  CountryCodeId = member.CountryCodeId,
                                  AnyValue = member.AnyValue,
                                  PhoneNo = $"+{member.CountryCodeId}{member.ContactNo}",
                                  SortOrder = member.SortOrder,
                                  Active = member.Active,
                                  CreatedBy = member.CreatedBy,
                                  CreatedDate = member.CreatedDate,
                                  UpdatedBy = member.UpdatedBy,
                                  UpdatedDate = member.UpdatedDate,
                                  MemberName = accepted != null ? accepted.Name : null,
                                  GroupName = member.CommitteGroup.GroupName,
                                  CommitteeName = member.CommitteGroup.Committee.CommitteeName,
                                  ImageName = member.FileStorage.FileName
                              })
                              .OrderBy(m => m.SortOrder)
                              .ThenBy(m => m.Name)
                              .ToList();

                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnData = result;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<CommitteMemberViewModel> GetCommitteeMemberById(long memberId)
        {
            var retModel = new ResponseEntity<CommitteMemberViewModel>();

            try
            {
                var memberData = _dbContext.CommitteMembers
                    .Include(m => m.FileStorage)
                    .Include(m => m.CommitteGroup)
                    .ThenInclude(g => g.Committee)
                    .SingleOrDefault(m => m.CommitteMemberId == memberId);

                if (memberData != null)
                {
                    var objModel = new CommitteMemberViewModel
                    {
                        CommitteMemberId = memberData.CommitteMemberId,
                        IssueId = memberData.IssueId,
                        GroupId = memberData.GroupId,
                        GroupName = memberData.CommitteGroup?.GroupName,
                        CommitteeName = memberData.CommitteGroup?.Committee?.CommitteeName,
                        PhoneNo = $"+{memberData.CountryCodeId}{memberData.ContactNo}",
                        Name = memberData.Name,
                        Position = memberData.Position,
                        ContactNo = memberData.ContactNo,
                        CountryCodeId = memberData.CountryCodeId,

                        SortOrder = memberData.SortOrder,
                        ImagePath = memberData.ImagePath,
                        ImageName = memberData.FileStorageId != null ? memberData.FileStorage.OrgFileName : null,
                        FileStorageId = memberData.FileStorageId,
                        AttachmentAny = !string.IsNullOrEmpty(memberData.ImagePath),
                        Active = memberData.Active,
                        CreatedBy = memberData.CreatedBy,
                        CreatedDate = memberData.CreatedDate,
                        UpdatedBy = memberData.UpdatedBy,
                        UpdatedDate = memberData.UpdatedDate
                    };

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnData = objModel;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.NotFound;
                    retModel.returnMessage = "Committee member not found.";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An error occurred while fetching the committee member.";
            }
            return retModel;
        }

        public async Task<ResponseEntity<bool>> UpdateCommitteeMember(CommitteMemberViewModel model)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var existingMember = await _dbContext.CommitteMembers
                    .Include(cm => cm.FileStorage)
                    .FirstOrDefaultAsync(cm => cm.CommitteMemberId == model.CommitteMemberId);

                if (existingMember == null)
                {
                    retModel.transactionStatus = HttpStatusCode.NotFound;
                    retModel.returnMessage = "Committee member not found.";
                    return retModel;
                }

                //var isDuplicate = await _dbContext.CommitteMembers
                //    .AnyAsync(cm =>
                //        cm.CommitteMemberId != model.CommitteMemberId &&
                //        cm.IssueId == model.IssueId &&
                //        cm.GroupId == model.GroupId);

                //if (isDuplicate)
                //{
                //    retModel.transactionStatus = HttpStatusCode.Conflict;
                //    retModel.returnMessage = "This member is already added to the selected group.";
                //    retModel.returnData = false;
                //    return retModel;
                //}

                existingMember.GroupId = model.GroupId;
                existingMember.Name = model.Name;
                existingMember.Position = model.Position;
                existingMember.CountryCodeId = model.CountryCodeId;
                existingMember.ContactNo = model.ContactNo;
                existingMember.SortOrder = model.SortOrder;
                existingMember.Active = true;
                existingMember.UpdatedBy = model.loggedinUserId;
                existingMember.UpdatedDate = DateTime.UtcNow;
                existingMember.AnyValue = model.AnyValue;


                if (model.Photo != null && model.Photo.Length > 0)
                {
                    var oldFileData = _dbContext.FileStorages.FirstOrDefault(f => f.FileStorageId == existingMember.FileStorageId);

                    if (oldFileData != null)
                    {
                        if (System.IO.File.Exists(oldFileData.StorageMode))
                            System.IO.File.Delete(oldFileData.StorageMode);

                        _dbContext.FileStorages.Remove(oldFileData);
                        await _dbContext.SaveChangesAsync();
                    }

                    var fileUploadResult = await SaveSingleFile(model.Photo, null, loggedInUser);

                    existingMember.ImagePath = fileUploadResult.returnData.FilePath;
                    existingMember.FileStorageId = fileUploadResult.returnData.FileStorageId;
                }
                _dbContext.CommitteMembers.Update(existingMember);
                await _dbContext.SaveChangesAsync();

                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnMessage = "Member updated successfully.";
                retModel.returnData = true;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = $"Error updating member: {ex.Message}";
                retModel.returnData = false;
            }
            return retModel;
        }

        public ResponseEntity<bool> DeleteMember(CommitteMemberViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var Details = _dbContext.CommitteMembers.Find(objModel.CommitteMemberId);

                if (Details.Active)
                {
                    Details.Active = false;

                    _dbContext.Entry(Details).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    Details.Active = true;
                    _dbContext.Entry(Details).State = EntityState.Modified;
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

        public async Task<ResponseEntity<List<CommitteByGroupDto>>> GetCommitteDetailsByGroup(long? GroupId)
        {
            var Retundata = new ResponseEntity<List<CommitteByGroupDto>>();
            try
            {
                var ObjData = _dbContext.CommitteMembers.Where(i => i.Active).ToList();
                if (ObjData != null)
                {
                    var CommitteeData = _dbContext.CommitteeGroups.Where(i => i.Active && i.GroupId == GroupId).
                            Select(i => new CommitteByGroupDto
                            {
                                GroupName = i.GroupName,
                                CommitteMembers = _dbContext.CommitteMembers.Where(c => c.GroupId == i.GroupId)
                                .Select(d => new CommitteDto
                                {
                                    ImagePath = d.ImagePath,
                                    MemberName = d.Name,
                                    Designation = d.Position,
                                    PhoneNumber = d.ContactNo.ToString(),
                                }).ToList(),
                            }).ToList();
                    Retundata.transactionStatus = System.Net.HttpStatusCode.OK;
                    Retundata.returnData = CommitteeData;
                    Retundata.returnMessage = "Success";
                }
                else
                {
                    Retundata.transactionStatus = System.Net.HttpStatusCode.NoContent;
                    Retundata.returnMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Retundata.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                Retundata.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return Retundata;
        }

        public async Task<ResponseEntity<List<CommitteeGroupList>>> GetAllGroupData()
        {
            var Retundata = new ResponseEntity<List<CommitteeGroupList>>();
            try
            {
                var CommitteeGroupData = _dbContext.CommitteeGroups.Where(i => i.Active).ToList();
                if (CommitteeGroupData != null)
                {
                    var GroupData = CommitteeGroupData.Where(i => i.Active).
                            Select(i => new CommitteeGroupList
                            {
                                GroupId = i.GroupId,
                                GroupName = i.GroupName,
                            }).ToList();
                    Retundata.transactionStatus = System.Net.HttpStatusCode.OK;
                    Retundata.returnData = GroupData;
                    Retundata.returnMessage = "Success";
                }
                else
                {
                    Retundata.transactionStatus = System.Net.HttpStatusCode.NoContent;
                    Retundata.returnMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Retundata.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                Retundata.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return Retundata;
        }
    }
}
