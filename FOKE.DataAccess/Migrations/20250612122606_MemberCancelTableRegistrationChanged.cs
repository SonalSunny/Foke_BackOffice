using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MemberCancelTableRegistrationChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.AlterColumn<long>(
                name: "RegistrationId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas",
                column: "RegistrationId",
                principalTable: "MembershipRequestDetails",
                principalColumn: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.AlterColumn<long>(
                name: "RegistrationId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas",
                column: "RegistrationId",
                principalTable: "MembershipRequestDetails",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
