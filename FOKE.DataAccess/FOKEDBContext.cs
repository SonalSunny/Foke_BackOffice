using FOKE.Entity;
using FOKE.Entity.AccountsData.DTO;
using FOKE.Entity.API.DeviceData.DTO;
using FOKE.Entity.AppInfoSectionData.DTO;
using FOKE.Entity.AreaMaster.DTO;
using FOKE.Entity.AreaMember.DTO;
using FOKE.Entity.CampaignData.DTO;
using FOKE.Entity.CancelReasonMaster.DTO;
using FOKE.Entity.CommitteeManagement.DTO;
using FOKE.Entity.ContactUs.DTO;
using FOKE.Entity.ContactUSFromWebsite.DTO;
using FOKE.Entity.DepartmentMaster.DTO;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.Localization.DTO;
using FOKE.Entity.MembershipData.DTO;
using FOKE.Entity.MembershipRegistration.DTO;
using FOKE.Entity.NewsAndEventsData.DTO;
using FOKE.Entity.Notification.DTO;
using FOKE.Entity.OfferData.DTO;
using FOKE.Entity.OperationManagement.DTO;
using FOKE.Entity.ProfessionData.DTO;
using FOKE.Entity.ProjectConfiguration.DTO;
using FOKE.Entity.RoleData.DTO;
using FOKE.Entity.SponsorshipData.DTO;
using FOKE.Entity.Task_Manager.DTO;
using FOKE.Entity.UnitData.DTO;
using FOKE.Entity.UnitMember;
using FOKE.Entity.WorkPlaceData.DTO;
using FOKE.Entity.ZoneMaster.DTO;
using FOKE.Entity.ZoneMember;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.DataAccess
{
    public class FOKEDBContext : DbContext
    {

        private readonly IHttpContextAccessor _Context;
        long? loggedInUser = null;

        public FOKEDBContext(DbContextOptions<FOKEDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _Context = httpContextAccessor;

            var claimsIdentity = _Context?.HttpContext?.User?.Identity as ClaimsIdentity;
            // _Logger = logger;
            var userIdentity = claimsIdentity?.Name;
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

        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleGroupClaim> RoleGroupClaims { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<LocalizationResource> LocalizationResources { get; set; }
        public DbSet<ProjectConfiguration> ProjectConfigurations { get; set; }
        public DbSet<LookupMaster> LookupMasters { get; set; }
        public DbSet<LookUpTypeMaster> LookupTypeMasters { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<WorkPlace> WorkPlace { get; set; }
        public DbSet<MembershipDetails> MembershipRequestDetails { get; set; }
        public DbSet<CancelReasonData> CancelReasonDatas { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<AreaData> AreaDatas { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<MembershipAccepted> MembershipAcceptedDatas { get; set; }
        public DbSet<MembershipRejected> MembershipRejectedDatas { get; set; }
        public DbSet<MemberShipCancelled> MemberShipCancelledDatas { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<FileMaster> FileMasters { get; set; }
        public DbSet<FolderMaster> FolderMasters { get; set; }
        public DbSet<FileStorage> FileStorages { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AttachmentData> Attachments { get; set; }
        public DbSet<MemberProfileData> MemberProfileDatas { get; set; }
        public DbSet<MembershipFee> MembershipFees { get; set; }
        public DbSet<ZoneMember> ZoneMembers { get; set; }
        public DbSet<UnitMember> UnitMembers { get; set; }
        public DbSet<AreaMember> AreaMembers { get; set; }
        public DbSet<DeviceDetails> DeviceDetails { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<NewsAndEvent> NewsAndEvents { get; set; }
        public DbSet<Committee> Committees { get; set; }
        public DbSet<Committegroup> CommitteeGroups { get; set; }
        public DbSet<CommitteMember> CommitteMembers { get; set; }
        public DbSet<ClientEnquieryData> ClientEnquieryDatas { get; set; }
        public DbSet<AppInfoSection> AppInfoSections { get; set; }
        public DbSet<Notification> NotificationDatas { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<Sponsorship> Sponsorships { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
            e.State == EntityState.Added
            || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {

                if (loggedInUser > 0)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                        ((BaseEntity)entityEntry.Entity).CreatedBy = loggedInUser;
                    }
                    else
                    {
                        ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;
                        ((BaseEntity)entityEntry.Entity).UpdatedBy = loggedInUser;
                    }

                }

            }
            var result = base.SaveChanges();
            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {

            var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
            e.State == EntityState.Added
            || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {

                if (loggedInUser > 0)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                        ((BaseEntity)entityEntry.Entity).CreatedBy = loggedInUser;
                    }
                    else
                    {
                        ((BaseEntity)entityEntry.Entity).UpdatedBy = loggedInUser;
                        ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;
                    }

                }


            }
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return result;
        }
    }
}

