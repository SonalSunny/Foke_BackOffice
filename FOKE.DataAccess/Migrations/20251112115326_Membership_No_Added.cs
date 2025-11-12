using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Membership_No_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembershipNo",
                table: "MembershipAcceptedDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipNo",
                table: "MembershipAcceptedDatas");
        }
    }
}
