using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ZoneMemberTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZoneMembers_MembershipAcceptedDatas_MemberId",
                table: "ZoneMembers");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "ZoneMembers",
                newName: "UserMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ZoneMembers_MemberId",
                table: "ZoneMembers",
                newName: "IX_ZoneMembers_UserMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneMembers_Users_UserMemberId",
                table: "ZoneMembers",
                column: "UserMemberId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZoneMembers_Users_UserMemberId",
                table: "ZoneMembers");

            migrationBuilder.RenameColumn(
                name: "UserMemberId",
                table: "ZoneMembers",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ZoneMembers_UserMemberId",
                table: "ZoneMembers",
                newName: "IX_ZoneMembers_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneMembers_MembershipAcceptedDatas_MemberId",
                table: "ZoneMembers",
                column: "MemberId",
                principalTable: "MembershipAcceptedDatas",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
