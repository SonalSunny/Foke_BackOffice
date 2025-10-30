using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOKE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImgaeUploadChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentModule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentMasterId = table.Column<long>(type: "bigint", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrgFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Attachments_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                });

            migrationBuilder.CreateTable(
                name: "MemberProfileDatas",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileStorageId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfileDatas", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_MemberProfileDatas_FileStorages_FileStorageId",
                        column: x => x.FileStorageId,
                        principalTable: "FileStorages",
                        principalColumn: "FileStorageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FileStorageId",
                table: "Attachments",
                column: "FileStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfileDatas_FileStorageId",
                table: "MemberProfileDatas",
                column: "FileStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "MemberProfileDatas");
        }
    }
}
