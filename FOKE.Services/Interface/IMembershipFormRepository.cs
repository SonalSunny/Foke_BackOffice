using FOKE.Entity;
using FOKE.Entity.API.DeviceLogin.ViewModel;
using FOKE.Entity.DashBoard;
using FOKE.Entity.MembershipData.DTO;
using FOKE.Entity.MembershipData.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Entity.MembershipRegistration.ViewModel;
using Microsoft.AspNetCore.Http;

namespace FOKE.Services.Interface
{
    public interface IMembershipFormRepository
    {
        Task<ResponseEntity<MembershipViewModel>> RegisterMember(MembershipViewModel model);
        ResponseEntity<List<MembershipViewModel>> GetAllMembers(long? areaid, long? proffessionID, long? workPlaceId);
        Task<ResponseEntity<PostMembershipViewModel>> IssueMember(PostMembershipViewModel model);
        ResponseEntity<List<PostMembershipViewModel>> GetAllAcceptedMembers(MemberListFilter InputData);
        ResponseEntity<PostMembershipViewModel> GetMemberById(long? id);
        ResponseEntity<List<PostMembershipViewModel>> GetAllRejectedMembers(long? Areaid, long? Proffesionid, long? workplaceId);
        ResponseEntity<List<PostMembershipViewModel>> GetAllCancelledMembers(long? Areaid, long? Proffesionid, long? workplaceId);
        long IsValidKuwaitCivilID(string civilId);
        long IsValidateKuwaitCivilID(string civilId, long? membershipId);
        long IsValidatePassportNo(string civilId);
        long IsValidateEditIssuePassportNo(string civilId, long? membershipId);
        bool IsValidContactNo(long ContactNo);
        bool IsValidEditIssueContactNo(long ContactNo, long? membershipId);
        long IsValidKuwaitCivilIDforEdit(string civilId, long? issueId);
        long IsValidatePassportNoforEdit(string civilId, long? issueId);
        bool IsValidContactNoforEdit(long ContactNo, long? issueId);
        Task<ResponseEntity<bool>> IssueMemberFromRegister(PostMembershipViewModel model);
        ResponseEntity<PostMembershipViewModel> GetAcceptedMemberById(long? id);
        ResponseEntity<PostMembershipViewModel> GetRejectedMemberById(long? id);
        ResponseEntity<List<PostMembershipViewModel>> GetMemberDetails(string? Keyword, string? SearchText);
        Task<ResponseEntity<PostMembershipViewModel>> UpdateRegisterdMember(PostMembershipViewModel model);

        ResponseEntity<string> ExportMembersDatatoExcel(long? Areaid, long? Proffesionid, long? workplaceId, long? Genders, long? BloodGroups, long? Dist, long? Departments, long? WorkYr, long? Zones, long? Units, long? AgeGroup, long? PaidStatus, string? searchField, string? keyword);
        ResponseEntity<string> ExportRejectedMemberstoExcel(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<string> ExportRejectedMemberstoPdf(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<string> ExportAllMemberstoPdf(long? Areaid, long? Proffesionid, long? workplaceId, long? Genders, long? BloodGroups, long? Dist, long? Departments, long? WorkYr, long? Zones, long? Units, long? AgeGroup, long? PaidStatus, string? searchField, string? keyword);
        ResponseEntity<List<PostMembershipViewModel>> GetAllMembersByArea(long? areaId, long? Proffesionid, long? workplaceId, long? UnitId, long? DepartmentId, long? ZoneId, long? userId, string? Value);
        ResponseEntity<List<PostMembershipViewModel>> GetAllMembersByUnit(long? unitId, long? Proffesionid, long? workplaceId, long? userId, string? Value);
        ResponseEntity<List<PostMembershipViewModel>> GetAllMembersByZone(long? zoneId, long? Proffesionid, long? workplaceId, long? userId, string? Value);
        void UpdateOtpPreferences(long issueId, bool isMobileOtp, bool isEmailOtp);
        Task<ResponseEntity<PostMembershipViewModel>> EditMembershipAsync(PostMembershipViewModel inputModel);
        Task<ResponseEntity<MemberDataViewModel>> GetMemberDataByCivilID(DeviceLoginViewModel InputData);
        Task<MembershipAccepted?> GetOtpSettingsRawAsync(long issueId);
        Task<ResponseEntity<bool>> CancelMembershipAsync(long memberId, string? reason, string? description);
        Task<ResponseEntity<bool>> UploadProfilePicture(IFormFile Input, string CivilID, string DeviceId, long devicePrimaryID);
        Task<ResponseEntity<MemberDetailsData>> GetMemberDataPostLogin(string CivilID, string DeviceId, long devicePrimaryID);
        ResponseEntity<string> ExportCancelledMemberstoExcel(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<string> ExportCancelledMemberstoPdf(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<string> ExportActiveMembersDatatoExcel(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<string> ExportActiveMemberstoPdf(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<List<PostMembershipViewModel>> GetAllActiveMembers(long? Areaid, long? Proffesionid, long? workplaceId);
        ResponseEntity<List<PostMembershipViewModel>> GetAllExpiredMembers(long? Areaid, long? Proffesionid, long? workplaceId);
        ResponseEntity<string> ExportExpiredMembersDatatoExcel(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<string> ExportExpiredMemberstoPdf(long? Areaid, long? Proffesionid, long? workplaceId, string? searchField, string? keyword);
        ResponseEntity<bool> ForceLogOutDevice(long? DeviceId);
        Task<ResponseEntity<PostMembershipViewModel>> UpdateRemark(long? Issueid, string? Remark);
        Task<ResponseEntity<MemberData>> GetMemberStatus(string CivilId);
        Task<ResponseEntity<bool>> SaveAttachment(List<IFormFile> fileInputs, long? AttachmentMasterId, long? CreatedBy);
       
        ResponseEntity<string> ExportMemberDatatoExcel(string search, long? area, long? unit, long? zone);
        ResponseEntity<string> ExportMembershipDatatoExcel(string search, long? campaign);
    }
}
