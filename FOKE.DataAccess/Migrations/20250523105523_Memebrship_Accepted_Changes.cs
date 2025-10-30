using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Memebrship_Accepted_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApprovedBy",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Memberfrom",
                table: "MembershipAcceptedDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipRequestedDate",
                table: "MembershipAcceptedDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReferredBy",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_ApprovedBy",
                table: "MembershipAcceptedDatas",
                column: "ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipAcceptedDatas_Users_ApprovedBy",
                table: "MembershipAcceptedDatas",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipAcceptedDatas_Users_ApprovedBy",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropIndex(
                name: "IX_MembershipAcceptedDatas_ApprovedBy",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "Memberfrom",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "MembershipRequestedDate",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "ReferredBy",
                table: "MembershipAcceptedDatas");
        }
    }
}
