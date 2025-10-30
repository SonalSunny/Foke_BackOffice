using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Issue_Membership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipAcceptedDatas",
                columns: table => new
                {
                    IssueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    ReferanceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<long>(type: "bigint", nullable: true),
                    BloodGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ProfessionId = table.Column<long>(type: "bigint", nullable: true),
                    WorkPlaceId = table.Column<long>(type: "bigint", nullable: true),
                    CountryCodeId = table.Column<long>(type: "bigint", nullable: true),
                    ContactNo = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    ZoneId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignAmount = table.Column<long>(type: "bigint", nullable: true),
                    AmountRecieved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HearAboutUsId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipAcceptedDatas", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_MembershipAcceptedDatas_AreaDatas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaDatas",
                        principalColumn: "AreaId");
                    table.ForeignKey(
                        name: "FK_MembershipAcceptedDatas_MembershipDetails_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "MembershipDetails",
                        principalColumn: "MembershipId");
                    table.ForeignKey(
                        name: "FK_MembershipAcceptedDatas_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "ProfessionId");
                    table.ForeignKey(
                        name: "FK_MembershipAcceptedDatas_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_MembershipAcceptedDatas_WorkPlace_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "WorkPlace",
                        principalColumn: "WorkPlaceId");
                    table.ForeignKey(
                        name: "FK_MembershipAcceptedDatas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId");
                });

            migrationBuilder.CreateTable(
                name: "MemberShipCancelledDatas",
                columns: table => new
                {
                    IssueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    ReferanceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<long>(type: "bigint", nullable: true),
                    BloodGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ProfessionId = table.Column<long>(type: "bigint", nullable: true),
                    WorkPlaceId = table.Column<long>(type: "bigint", nullable: true),
                    CountryCode = table.Column<long>(type: "bigint", nullable: true),
                    ContactNo = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    ZoneId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignAmount = table.Column<long>(type: "bigint", nullable: true),
                    AmountRecieved = table.Column<long>(type: "bigint", nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HearAboutUsId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberShipCancelledDatas", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_MemberShipCancelledDatas_AreaDatas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaDatas",
                        principalColumn: "AreaId");
                    table.ForeignKey(
                        name: "FK_MemberShipCancelledDatas_MembershipDetails_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "MembershipDetails",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberShipCancelledDatas_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "ProfessionId");
                    table.ForeignKey(
                        name: "FK_MemberShipCancelledDatas_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_MemberShipCancelledDatas_WorkPlace_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "WorkPlace",
                        principalColumn: "WorkPlaceId");
                    table.ForeignKey(
                        name: "FK_MemberShipCancelledDatas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId");
                });

            migrationBuilder.CreateTable(
                name: "MembershipRejectedDatas",
                columns: table => new
                {
                    IssueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    ReferanceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<long>(type: "bigint", nullable: true),
                    BloodGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ProfessionId = table.Column<long>(type: "bigint", nullable: true),
                    WorkPlaceId = table.Column<long>(type: "bigint", nullable: true),
                    CountryCodeId = table.Column<long>(type: "bigint", nullable: true),
                    ContactNo = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    ZoneId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignAmount = table.Column<long>(type: "bigint", nullable: true),
                    AmountRecieved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HearAboutUsId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipRejectedDatas", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_MembershipRejectedDatas_AreaDatas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaDatas",
                        principalColumn: "AreaId");
                    table.ForeignKey(
                        name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "MembershipDetails",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipRejectedDatas_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "ProfessionId");
                    table.ForeignKey(
                        name: "FK_MembershipRejectedDatas_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_MembershipRejectedDatas_WorkPlace_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "WorkPlace",
                        principalColumn: "WorkPlaceId");
                    table.ForeignKey(
                        name: "FK_MembershipRejectedDatas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_AreaId",
                table: "MembershipAcceptedDatas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_ProfessionId",
                table: "MembershipAcceptedDatas",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_RegistrationId",
                table: "MembershipAcceptedDatas",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_UnitId",
                table: "MembershipAcceptedDatas",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_ZoneId",
                table: "MembershipAcceptedDatas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_AreaId",
                table: "MemberShipCancelledDatas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_ProfessionId",
                table: "MemberShipCancelledDatas",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_RegistrationId",
                table: "MemberShipCancelledDatas",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_UnitId",
                table: "MemberShipCancelledDatas",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_ZoneId",
                table: "MemberShipCancelledDatas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_AreaId",
                table: "MembershipRejectedDatas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_ProfessionId",
                table: "MembershipRejectedDatas",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_UnitId",
                table: "MembershipRejectedDatas",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_ZoneId",
                table: "MembershipRejectedDatas",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembershipAcceptedDatas");

            migrationBuilder.DropTable(
                name: "MemberShipCancelledDatas");

            migrationBuilder.DropTable(
                name: "MembershipRejectedDatas");
        }
    }
}
