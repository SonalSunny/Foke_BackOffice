using FOKE.DataAccess;
using FOKE.Services.Interface;
using FOKE.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace FOKE.Program
{
    public static class DBContextInjector
    {
        public static void RegisterDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FOKEDBContext>(options =>
            options.UseSqlServer(connectionString));

            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDropDownRepository, DropDownRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProjectConfigurationRepository, ProjectConfigurationRepository>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IProfessionRepository, ProfessionRepository>();
            services.AddScoped<IWorkPlaceRepository, WorkPlaceRepository>();
            services.AddScoped<IMembershipFormRepository, MembershipFormRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IFileMasterRepository, FileMasterRepository>();
            services.AddScoped<IFolderMasterRepository, FolderMasterRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IAuthenticationServiceRepository, AuthenticationServiceRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<INewsAndEventsRepository, NewsAndEventsRepository>();
            services.AddScoped<ICommitteeRepository, CommitteeRepository>();
            services.AddScoped<ICommitteGroupRepository, CommitteGroupRepository>();
            services.AddScoped<ICommitteeMemberRepository, CommitteeMemberRepository>();
            services.AddScoped<IcontactUsRepository, ContactUsRepository>();
            services.AddScoped<IappInfoSectionRepository, AppInfoSectionRepository>();
            services.AddScoped<IfireBaseNotificationService, FireBaseNotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IFeeCollectionReport, FeeCollectionRepository>();
            services.AddScoped<ISponsorshipRepository, SponsorshipRepository>();
            services.AddScoped<IAccountsRepostory, AccountsRepostory>();
        }
    }
}
