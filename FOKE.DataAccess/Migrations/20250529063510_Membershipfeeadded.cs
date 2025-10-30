using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Membershipfeeadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "MembershipAcceptedDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentReceivedBy",
                table: "MembershipAcceptedDatas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CollectionAdded",
                table: "Campaigns",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MembershipFees",
                columns: table => new
                {
                    MembershipFeeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<long>(type: "bigint", nullable: true),
                    Campaign = table.Column<long>(type: "bigint", nullable: true),
                    AmountToPay = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentType = table.Column<long>(type: "bigint", nullable: true),
                    PaymentReceivedBy = table.Column<long>(type: "bigint", nullable: true),
                    CollectionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipFees", x => x.MembershipFeeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "PaymentReceivedBy",
                table: "MembershipAcceptedDatas");

            migrationBuilder.DropColumn(
                name: "CollectionAdded",
                table: "Campaigns");
        }
    }
}
