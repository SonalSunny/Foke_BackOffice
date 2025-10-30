using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AreaMembersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaMembers",
                columns: table => new
                {
                    AreaMemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaMembers", x => x.AreaMemberId);
                    table.ForeignKey(
                        name: "FK_AreaMembers_AreaDatas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaDatas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaMembers_MembershipAcceptedDatas_MemberId",
                        column: x => x.MemberId,
                        principalTable: "MembershipAcceptedDatas",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitMembers",
                columns: table => new
                {
                    UnitMemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitMembers", x => x.UnitMemberId);
                    table.ForeignKey(
                        name: "FK_UnitMembers_MembershipAcceptedDatas_MemberId",
                        column: x => x.MemberId,
                        principalTable: "MembershipAcceptedDatas",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitMembers_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZoneMembers",
                columns: table => new
                {
                    ZoneMemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneMembers", x => x.ZoneMemberId);
                    table.ForeignKey(
                        name: "FK_ZoneMembers_MembershipAcceptedDatas_MemberId",
                        column: x => x.MemberId,
                        principalTable: "MembershipAcceptedDatas",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZoneMembers_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaMembers_AreaId",
                table: "AreaMembers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMembers_MemberId",
                table: "AreaMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitMembers_MemberId",
                table: "UnitMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitMembers_UnitId",
                table: "UnitMembers",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneMembers_MemberId",
                table: "ZoneMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneMembers_ZoneId",
                table: "ZoneMembers",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaMembers");

            migrationBuilder.DropTable(
                name: "UnitMembers");

            migrationBuilder.DropTable(
                name: "ZoneMembers");
        }
    }
}
