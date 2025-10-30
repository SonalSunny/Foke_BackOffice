using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UnitMemberChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitMembers_MembershipAcceptedDatas_MemberId",
                table: "UnitMembers");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "UnitMembers",
                newName: "UserMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitMembers_MemberId",
                table: "UnitMembers",
                newName: "IX_UnitMembers_UserMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitMembers_Users_UserMemberId",
                table: "UnitMembers",
                column: "UserMemberId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitMembers_Users_UserMemberId",
                table: "UnitMembers");

            migrationBuilder.RenameColumn(
                name: "UserMemberId",
                table: "UnitMembers",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitMembers_UserMemberId",
                table: "UnitMembers",
                newName: "IX_UnitMembers_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitMembers_MembershipAcceptedDatas_MemberId",
                table: "UnitMembers",
                column: "MemberId",
                principalTable: "MembershipAcceptedDatas",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
