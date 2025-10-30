using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class membershipdetailchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProffessionOther",
                table: "MembershipDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkYear",
                table: "MembershipDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkplaceOther",
                table: "MembershipDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProffessionOther",
                table: "MembershipDetails");

            migrationBuilder.DropColumn(
                name: "WorkYear",
                table: "MembershipDetails");

            migrationBuilder.DropColumn(
                name: "WorkplaceOther",
                table: "MembershipDetails");
        }
    }
}
