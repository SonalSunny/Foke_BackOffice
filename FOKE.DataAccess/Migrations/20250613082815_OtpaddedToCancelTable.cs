using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OtpaddedToCancelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailOtp",
                table: "MemberShipCancelledDatas",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MobileOtp",
                table: "MemberShipCancelledDatas",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOtp",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "MobileOtp",
                table: "MemberShipCancelledDatas");
        }
    }
}
