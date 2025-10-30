using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeviceDetailsFileStorageIDChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrgFileName",
                table: "FileStorages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "DeviceDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileStorageId",
                table: "DeviceDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrgFileName",
                table: "DeviceDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDetails_FileStorageId",
                table: "DeviceDetails",
                column: "FileStorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDetails_FileStorages_FileStorageId",
                table: "DeviceDetails",
                column: "FileStorageId",
                principalTable: "FileStorages",
                principalColumn: "FileStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDetails_FileStorages_FileStorageId",
                table: "DeviceDetails");

            migrationBuilder.DropIndex(
                name: "IX_DeviceDetails_FileStorageId",
                table: "DeviceDetails");

            migrationBuilder.DropColumn(
                name: "OrgFileName",
                table: "FileStorages");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "DeviceDetails");

            migrationBuilder.DropColumn(
                name: "FileStorageId",
                table: "DeviceDetails");

            migrationBuilder.DropColumn(
                name: "OrgFileName",
                table: "DeviceDetails");
        }
    }
}
