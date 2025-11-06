using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class request_table_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "MembershipRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactCountryCodeid",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactEmail",
                table: "MembershipRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "MembershipRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactNumber",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmergencyContactRelation",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermenantAddress",
                table: "MembershipRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "MembershipRequestDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "EmergencyContactCountryCodeid",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "EmergencyContactEmail",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelation",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "PermenantAddress",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "MembershipRequestDetails");
        }
    }
}
