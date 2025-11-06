using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ParentId_Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "MinorApplicantDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RelationType",
                table: "MinorApplicantDetails",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MinorApplicantDetails");

            migrationBuilder.DropColumn(
                name: "RelationType",
                table: "MinorApplicantDetails");
        }
    }
}
