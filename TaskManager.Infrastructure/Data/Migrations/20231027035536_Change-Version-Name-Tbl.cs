#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVersionNameTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Version_VersionId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Version_Projects_ProjectId",
                table: "Version");

            migrationBuilder.DropForeignKey(
                name: "FK_Version_Statuses_StatusId",
                table: "Version");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Version",
                table: "Version");

            migrationBuilder.RenameTable(
                name: "Version",
                newName: "Versions");

            migrationBuilder.RenameIndex(
                name: "IX_Version_StatusId",
                table: "Versions",
                newName: "IX_Versions_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Version_ProjectId",
                table: "Versions",
                newName: "IX_Versions_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Versions",
                table: "Versions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Versions_VersionId",
                table: "Issues",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Projects_ProjectId",
                table: "Versions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Statuses_StatusId",
                table: "Versions",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Versions_VersionId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Projects_ProjectId",
                table: "Versions");

            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Statuses_StatusId",
                table: "Versions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Versions",
                table: "Versions");

            migrationBuilder.RenameTable(
                name: "Versions",
                newName: "Version");

            migrationBuilder.RenameIndex(
                name: "IX_Versions_StatusId",
                table: "Version",
                newName: "IX_Version_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Versions_ProjectId",
                table: "Version",
                newName: "IX_Version_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Version",
                table: "Version",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Version_VersionId",
                table: "Issues",
                column: "VersionId",
                principalTable: "Version",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Version_Projects_ProjectId",
                table: "Version",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Version_Statuses_StatusId",
                table: "Version",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
