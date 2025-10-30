using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MemAcceptCanclUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipAcceptedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropIndex(
                name: "IX_MemberShipCancelledDatas_RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropIndex(
                name: "IX_MembershipAcceptedDatas_RegistrationId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "MembershipAcceptedDatas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RegistrationId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegistrationId",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_RegistrationId",
                table: "MemberShipCancelledDatas",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_RegistrationId",
                table: "MembershipAcceptedDatas",
                column: "RegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipAcceptedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipAcceptedDatas",
                column: "RegistrationId",
                principalTable: "MembershipRequestDetails",
                principalColumn: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas",
                column: "RegistrationId",
                principalTable: "MembershipRequestDetails",
                principalColumn: "MembershipId");
        }
    }
}
