using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MembershipCancelledTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_WorkPlace_ProfessionId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.RenameColumn(
                name: "RejectionRemarks",
                table: "MemberShipCancelledDatas",
                newName: "WorkplaceOther");

            migrationBuilder.RenameColumn(
                name: "RejectionReasonId",
                table: "MemberShipCancelledDatas",
                newName: "WorkYear");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "MemberShipCancelledDatas",
                newName: "ProffessionOther");

            migrationBuilder.AddColumn<long>(
                name: "ApprovedBy",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CancelReasonId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationRemarks",
                table: "MemberShipCancelledDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CancelledBy",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Memberfrom",
                table: "MemberShipCancelledDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipRequestedDate",
                table: "MemberShipCancelledDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "MemberShipCancelledDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentReceivedBy",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReferredBy",
                table: "MemberShipCancelledDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_ApprovedBy",
                table: "MemberShipCancelledDatas",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_CancelledBy",
                table: "MemberShipCancelledDatas",
                column: "CancelledBy");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_CancelReasonId",
                table: "MemberShipCancelledDatas",
                column: "CancelReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShipCancelledDatas_WorkPlaceId",
                table: "MemberShipCancelledDatas",
                column: "WorkPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_CancelReasonDatas_CancelReasonId",
                table: "MemberShipCancelledDatas",
                column: "CancelReasonId",
                principalTable: "CancelReasonDatas",
                principalColumn: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_Users_ApprovedBy",
                table: "MemberShipCancelledDatas",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_Users_CancelledBy",
                table: "MemberShipCancelledDatas",
                column: "CancelledBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_WorkPlace_WorkPlaceId",
                table: "MemberShipCancelledDatas",
                column: "WorkPlaceId",
                principalTable: "WorkPlace",
                principalColumn: "WorkPlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_CancelReasonDatas_CancelReasonId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_Users_ApprovedBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_Users_CancelledBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_WorkPlace_WorkPlaceId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropIndex(
                name: "IX_MemberShipCancelledDatas_ApprovedBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropIndex(
                name: "IX_MemberShipCancelledDatas_CancelledBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropIndex(
                name: "IX_MemberShipCancelledDatas_CancelReasonId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropIndex(
                name: "IX_MemberShipCancelledDatas_WorkPlaceId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "CancelReasonId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "CancellationRemarks",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "Memberfrom",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "MembershipRequestedDate",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "PaymentReceivedBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropColumn(
                name: "ReferredBy",
                table: "MemberShipCancelledDatas");

            migrationBuilder.RenameColumn(
                name: "WorkplaceOther",
                table: "MemberShipCancelledDatas",
                newName: "RejectionRemarks");

            migrationBuilder.RenameColumn(
                name: "WorkYear",
                table: "MemberShipCancelledDatas",
                newName: "RejectionReasonId");

            migrationBuilder.RenameColumn(
                name: "ProffessionOther",
                table: "MemberShipCancelledDatas",
                newName: "RejectionReason");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_WorkPlace_ProfessionId",
                table: "MemberShipCancelledDatas",
                column: "ProfessionId",
                principalTable: "WorkPlace",
                principalColumn: "WorkPlaceId");
        }
    }
}
