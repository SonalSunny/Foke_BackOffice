using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AreaMemberTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaMembers_MembershipAcceptedDatas_MemberId",
                table: "AreaMembers");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "AreaMembers",
                newName: "UserMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_AreaMembers_MemberId",
                table: "AreaMembers",
                newName: "IX_AreaMembers_UserMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_AreaMembers_Users_UserMemberId",
                table: "AreaMembers",
                column: "UserMemberId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaMembers_Users_UserMemberId",
                table: "AreaMembers");

            migrationBuilder.RenameColumn(
                name: "UserMemberId",
                table: "AreaMembers",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_AreaMembers_UserMemberId",
                table: "AreaMembers",
                newName: "IX_AreaMembers_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_AreaMembers_MembershipAcceptedDatas_MemberId",
                table: "AreaMembers",
                column: "MemberId",
                principalTable: "MembershipAcceptedDatas",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
