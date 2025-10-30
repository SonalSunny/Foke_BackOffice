using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentAddedandTableNameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipAcceptedDatas_MembershipDetails_RegistrationId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipDetails_RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipDetails",
                table: "MembershipDetails");

            migrationBuilder.RenameTable(
                name: "MembershipDetails",
                newName: "MembershipRequestDetails");

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "MembershipRequestDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipRequestDetails",
                table: "MembershipRequestDetails",
                column: "MembershipId");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

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
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId",
                principalTable: "MembershipRequestDetails",
                principalColumn: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipAcceptedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipRequestDetails_RegistrationId",
                table: "MemberShipCancelledDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipRequestDetails_RegistrationId",
                table: "MembershipRejectedDatas");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipRequestDetails",
                table: "MembershipRequestDetails");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "MembershipRequestDetails");

            migrationBuilder.RenameTable(
                name: "MembershipRequestDetails",
                newName: "MembershipDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipDetails",
                table: "MembershipDetails",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipAcceptedDatas_MembershipDetails_RegistrationId",
                table: "MembershipAcceptedDatas",
                column: "RegistrationId",
                principalTable: "MembershipDetails",
                principalColumn: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShipCancelledDatas_MembershipDetails_RegistrationId",
                table: "MemberShipCancelledDatas",
                column: "RegistrationId",
                principalTable: "MembershipDetails",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRejectedDatas_MembershipDetails_RegistrationId",
                table: "MembershipRejectedDatas",
                column: "RegistrationId",
                principalTable: "MembershipDetails",
                principalColumn: "MembershipId");
        }
    }
}
