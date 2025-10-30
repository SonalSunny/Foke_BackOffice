using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Reject_Member_Data_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                table: "MembershipRejectedDatas");

            migrationBuilder.AlterColumn<long>(
                name: "RegistrationId",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId",
                principalTable: "MembershipDetails",
                principalColumn: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                table: "MembershipRejectedDatas");

            migrationBuilder.AlterColumn<long>(
                name: "RegistrationId",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId",
                principalTable: "MembershipDetails",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
