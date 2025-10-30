using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Composite_unique_Key_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MembershipFees_MemberID_Campaign",
                table: "MembershipFees",
                columns: new[] { "MemberID", "Campaign" },
                unique: true,
                filter: "[MemberID] IS NOT NULL AND [Campaign] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MembershipFees_MemberID_Campaign",
                table: "MembershipFees");
        }
    }
}
