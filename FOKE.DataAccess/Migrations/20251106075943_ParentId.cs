using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KuwaitAddres",
                table: "MinorApplicantDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermenantAddress",
                table: "MinorApplicantDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "MinorApplicantDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KuwaitAddres",
                table: "MinorApplicantDetails");

            migrationBuilder.DropColumn(
                name: "PermenantAddress",
                table: "MinorApplicantDetails");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "MinorApplicantDetails");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MembershipRequestDetails");
        }
    }
}
