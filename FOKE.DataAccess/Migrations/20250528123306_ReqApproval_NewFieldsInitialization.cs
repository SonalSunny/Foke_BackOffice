using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReqApproval_NewFieldsInitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProffessionOther",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkYear",
                table: "MembershipRejectedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkplaceOther",
                table: "MembershipRejectedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProffessionOther",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkYear",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkplaceOther",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "ProffessionOther",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "WorkYear",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "WorkplaceOther",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "ProffessionOther",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "WorkYear",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "WorkplaceOther",
                table: "MembershipAcceptedDatas");
        }
    }
}
