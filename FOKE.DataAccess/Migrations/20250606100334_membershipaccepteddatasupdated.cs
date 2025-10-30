using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class membershipaccepteddatasupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailOtp",
                table: "MembershipAcceptedDatas",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MobileOtp",
                table: "MembershipAcceptedDatas",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOtp",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "MobileOtp",
                table: "MembershipAcceptedDatas");
        }
    }
}
