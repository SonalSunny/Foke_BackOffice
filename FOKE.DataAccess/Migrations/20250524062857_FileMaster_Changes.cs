using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileMaster_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileStorages",
                columns: table => new
                {
                    FileStorageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentLength = table.Column<long>(type: "bigint", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorages", x => x.FileStorageId);
                });

            migrationBuilder.CreateTable(
                name: "FolderMasters",
                columns: table => new
                {
                    FolderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderMasters", x => x.FolderId);
                    table.ForeignKey(
                        name: "FK_FolderMasters_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                });

            migrationBuilder.CreateTable(
                name: "FileMasters",
                columns: table => new
                {
                    FileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FolderId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileRefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMasters", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileMasters_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                    table.ForeignKey(
                        name: "FK_FileMasters_FolderMasters_FolderId",
                        column: x => x.FolderId,
                        principalTable: "FolderMasters",
                        principalColumn: "FolderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileMasters_FileStorageId",
                table: "FileMasters",
                column: "FileStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMasters_FolderId",
                table: "FileMasters",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderMasters_FileStorageId",
                table: "FolderMasters",
                column: "FileStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileMasters");

            migrationBuilder.DropTable(
                name: "FolderMasters");

            migrationBuilder.DropTable(
                name: "FileStorages");
        }
    }
}
