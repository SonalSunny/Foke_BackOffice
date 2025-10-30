using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastClosedDateTime",
                table: "DeviceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOpenDateTime",
                table: "DeviceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppInfoSections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionType = table.Column<long>(type: "bigint", nullable: true),
                    HTMLContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowInWebsite = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMobile = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInfoSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientEnquieryDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DevicePrimaryId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEnquieryDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Committees",
                columns: table => new
                {
                    CommitteeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommitteeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committees", x => x.CommitteeId);
                });

            migrationBuilder.CreateTable(
                name: "NewsAndEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShowInWebsite = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMobile = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<long>(type: "bigint", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsAndEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsAndEvents_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                });

            migrationBuilder.CreateTable(
                name: "NotificationDatas",
                columns: table => new
                {
                    NotificationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendTo = table.Column<long>(type: "bigint", nullable: true),
                    SendToNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    ZoneId = table.Column<long>(type: "bigint", nullable: true),
                    LogGeneratedStatus = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDatas", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_NotificationDatas_AreaDatas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaDatas",
                        principalColumn: "AreaId");
                    table.ForeignKey(
                        name: "FK_NotificationDatas_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_NotificationDatas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInWebsite = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMobile = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_Offers_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                });

            migrationBuilder.CreateTable(
                name: "CommitteeGroups",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommitteeId = table.Column<long>(type: "bigint", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeGroups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_CommitteeGroups_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId");
                });

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<long>(type: "bigint", nullable: true),
                    FcmToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirebaseSuccess = table.Column<bool>(type: "bit", nullable: false),
                    FirebaseError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationLogs_NotificationDatas_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "NotificationDatas",
                        principalColumn: "NotificationId");
                });

            migrationBuilder.CreateTable(
                name: "CommitteMembers",
                columns: table => new
                {
                    CommitteMemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<long>(type: "bigint", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCodeId = table.Column<long>(type: "bigint", nullable: true),
                    ContactNo = table.Column<long>(type: "bigint", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteMembers", x => x.CommitteMemberId);
                    table.ForeignKey(
                        name: "FK_CommitteMembers_CommitteeGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CommitteeGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_CommitteMembers_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                    table.ForeignKey(
                        name: "FK_CommitteMembers_MembershipAcceptedDatas_IssueId",
                        column: x => x.IssueId,
                        principalTable: "MembershipAcceptedDatas",
                        principalColumn: "IssueId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeGroups_CommitteeId",
                table: "CommitteeGroups",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteMembers_FileStorageId",
                table: "CommitteMembers",
                column: "FileStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteMembers_GroupId",
                table: "CommitteMembers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteMembers_IssueId",
                table: "CommitteMembers",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsAndEvents_FileStorageId",
                table: "NewsAndEvents",
                column: "FileStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDatas_AreaId",
                table: "NotificationDatas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDatas_UnitId",
                table: "NotificationDatas",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDatas_ZoneId",
                table: "NotificationDatas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_NotificationId",
                table: "NotificationLogs",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_FileStorageId",
                table: "Offers",
                column: "FileStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppInfoSections");

            migrationBuilder.DropTable(
                name: "ClientEnquieryDatas");

            migrationBuilder.DropTable(
                name: "CommitteMembers");

            migrationBuilder.DropTable(
                name: "NewsAndEvents");

            migrationBuilder.DropTable(
                name: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "CommitteeGroups");

            migrationBuilder.DropTable(
                name: "NotificationDatas");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropColumn(
                name: "LastClosedDateTime",
                table: "DeviceDetails");

            migrationBuilder.DropColumn(
                name: "LastOpenDateTime",
                table: "DeviceDetails");
        }
    }
}
