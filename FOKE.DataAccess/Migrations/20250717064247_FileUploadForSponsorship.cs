using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileUploadForSponsorship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FileStorageId",
                table: "Sponsorships",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Sponsorships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorships_FileStorageId",
                table: "Sponsorships",
                column: "FileStorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorships_FileStorages_FileStorageId",
                table: "Sponsorships",
                column: "FileStorageId",
                principalTable: "FileStorages",
                principalColumn: "FileStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorships_FileStorages_FileStorageId",
                table: "Sponsorships");

            migrationBuilder.DropIndex(
                name: "IX_Sponsorships_FileStorageId",
                table: "Sponsorships");

            migrationBuilder.DropColumn(
                name: "FileStorageId",
                table: "Sponsorships");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Sponsorships");
        }
    }
}
