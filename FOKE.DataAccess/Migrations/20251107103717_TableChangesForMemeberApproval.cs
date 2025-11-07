using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TableChangesForMemeberApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropIndex(
                name: "IX_MembershipRejectedDatas_RegistrationId",
                table: "MembershipRejectedDatas");

            migrationBuilder.RenameColumn(
                name: "RegistrationId",
                table: "MembershipRejectedDatas",
                newName: "WhatsAppNoCountryCodeid");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactCountryCodeid",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactEmail",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactNumber",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactRelation",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KuwaitAddres",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MembershipType",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermenantAddress",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNo",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactCountryCodeid",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactEmail",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactNumber",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactRelation",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KuwaitAddres",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MembershipType",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermenantAddress",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNo",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNoCountryCodeid",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactCountryCodeid",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactEmail",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactNumber",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactRelation",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KuwaitAddres",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MembershipType",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermenantAddress",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNo",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhatsAppNoCountryCodeid",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MinorApplicantRejectedDatas",
                columns: table => new
                {
                    MembershipId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationType = table.Column<long>(type: "bigint", nullable: true),
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
                    KuwaitAddres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenantAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinorApplicantRejectedDatas", x => x.MembershipId);
                });

            migrationBuilder.CreateTable(
                name: "MinorApplicantsAcceptedDatas",
                columns: table => new
                {
                    MembershipId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationType = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    KuwaitAddres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenantAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinorApplicantsAcceptedDatas", x => x.MembershipId);
                });

            migrationBuilder.CreateTable(
                name: "MinorApplicantsCancelledDatas",
                columns: table => new
                {
                    MembershipId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationType = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<long>(type: "bigint", nullable: true),
                    BloodGroupId = table.Column<long>(type: "bigint", nullable: true),
                    CountryCode = table.Column<long>(type: "bigint", nullable: true),
                    ContactNo = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    KuwaitAddres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermenantAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinorApplicantsCancelledDatas", x => x.MembershipId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MinorApplicantsAcceptedDatas_CivilId",
                table: "MinorApplicantsAcceptedDatas",
                column: "CivilId",
                unique: true,
                filter: "[CivilId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinorApplicantRejectedDatas");

            migrationBuilder.DropTable(
                name: "MinorApplicantsAcceptedDatas");

            migrationBuilder.DropTable(
                name: "MinorApplicantsCancelledDatas");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactCountryCodeid",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactEmail",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelation",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "KuwaitAddres",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "PermenantAddress",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "WhatsAppNo",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactCountryCodeid",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactEmail",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelation",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "KuwaitAddres",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "PermenantAddress",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "WhatsAppNo",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "WhatsAppNoCountryCodeid",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactCountryCodeid",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactEmail",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelation",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "KuwaitAddres",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "PermenantAddress",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "WhatsAppNo",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "WhatsAppNoCountryCodeid",
                table: "MembershipAcceptedDatas");

            migrationBuilder.RenameColumn(
                name: "WhatsAppNoCountryCodeid",
                table: "MembershipRejectedDatas",
                newName: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId",
                principalTable: "MembershipRequestDetails",
                principalColumn: "MembershipId");
        }
    }
}
