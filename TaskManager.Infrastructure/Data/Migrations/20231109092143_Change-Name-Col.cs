#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRoles_Permissions_PermissionGroupId",
                table: "PermissionRoles");

            migrationBuilder.RenameColumn(
                name: "PermissionGroupId",
                table: "PermissionRoles",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionRoles_PermissionGroupId",
                table: "PermissionRoles",
                newName: "IX_PermissionRoles_PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRoles_Permissions_PermissionId",
                table: "PermissionRoles",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRoles_Permissions_PermissionId",
                table: "PermissionRoles");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "PermissionRoles",
                newName: "PermissionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionRoles_PermissionId",
                table: "PermissionRoles",
                newName: "IX_PermissionRoles_PermissionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRoles_Permissions_PermissionGroupId",
                table: "PermissionRoles",
                column: "PermissionGroupId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
