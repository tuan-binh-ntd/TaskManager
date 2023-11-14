using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionGroupTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRoles_AppRole_RoleId",
                table: "PermissionRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Priorities_Projects_ProjectId",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AppRole");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "PermissionRoles",
                newName: "PermissionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionRoles_RoleId",
                table: "PermissionRoles",
                newName: "IX_PermissionRoles_PermissionGroupId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Priorities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionGroups_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_ProjectId",
                table: "PermissionGroups",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRoles_PermissionGroups_PermissionGroupId",
                table: "PermissionRoles",
                column: "PermissionGroupId",
                principalTable: "PermissionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Priorities_Projects_ProjectId",
                table: "Priorities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRoles_PermissionGroups_PermissionGroupId",
                table: "PermissionRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Priorities_Projects_ProjectId",
                table: "Priorities");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.RenameColumn(
                name: "PermissionGroupId",
                table: "PermissionRoles",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionRoles_PermissionGroupId",
                table: "PermissionRoles",
                newName: "IX_PermissionRoles_RoleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Priorities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "AppRole",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRoles_AppRole_RoleId",
                table: "PermissionRoles",
                column: "RoleId",
                principalTable: "AppRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Priorities_Projects_ProjectId",
                table: "Priorities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
