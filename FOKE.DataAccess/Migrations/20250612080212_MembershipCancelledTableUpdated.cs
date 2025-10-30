using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MembershipCancelledTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RejectionReasonId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionRemarks",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "RejectionReasonId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "RejectionRemarks",
                table: "MemberShipCancelledDatas");
        }
    }
}
