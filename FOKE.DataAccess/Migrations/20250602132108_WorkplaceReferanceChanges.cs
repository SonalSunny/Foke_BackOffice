using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WorkplaceReferanceChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipAcceptedDatas_WorkPlace_ProfessionId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_WorkPlace_ProfessionId",
                table: "MembershipRejectedDatas");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRejectedDatas_WorkPlaceId",
                table: "MembershipRejectedDatas",
                column: "WorkPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipAcceptedDatas_WorkPlaceId",
                table: "MembershipAcceptedDatas",
                column: "WorkPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipAcceptedDatas_WorkPlace_WorkPlaceId",
                table: "MembershipAcceptedDatas",
                column: "WorkPlaceId",
                principalTable: "WorkPlace",
                principalColumn: "WorkPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_WorkPlace_WorkPlaceId",
                table: "MembershipRejectedDatas",
                column: "WorkPlaceId",
                principalTable: "WorkPlace",
                principalColumn: "WorkPlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipAcceptedDatas_WorkPlace_WorkPlaceId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_WorkPlace_WorkPlaceId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropIndex(
                name: "IX_MembershipRejectedDatas_WorkPlaceId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropIndex(
                name: "IX_MembershipAcceptedDatas_WorkPlaceId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipAcceptedDatas_WorkPlace_ProfessionId",
                table: "MembershipAcceptedDatas",
                column: "ProfessionId",
                principalTable: "WorkPlace",
                principalColumn: "WorkPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_WorkPlace_ProfessionId",
                table: "MembershipRejectedDatas",
                column: "ProfessionId",
                principalTable: "WorkPlace",
                principalColumn: "WorkPlaceId");
        }
    }
}
