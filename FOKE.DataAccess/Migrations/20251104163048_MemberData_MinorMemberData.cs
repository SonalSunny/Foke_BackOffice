using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MemberData_MinorMemberData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KuwaitAddres",
                table: "MembershipRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MembershipType",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNo",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNoCountryCodeid",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MinorApplicantDetails",
                columns: table => new
                {
                    MembershipId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<long>(type: "bigint", nullable: true),
                    BloodGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ProffessionId = table.Column<long>(type: "bigint", nullable: true),
                    CountryCode = table.Column<long>(type: "bigint", nullable: true),
                    ContactNo = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    ProffessionOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinorApplicantDetails", x => x.MembershipId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinorApplicantDetails");

            migrationBuilder.DropColumn(
                name: "KuwaitAddres",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "WhatsAppNo",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "WhatsAppNoCountryCodeid",
                table: "MembershipRequestDetails");
        }
    }
}
