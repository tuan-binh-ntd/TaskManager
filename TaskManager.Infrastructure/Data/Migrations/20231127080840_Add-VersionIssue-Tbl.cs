using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVersionIssueTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Versions_VersionId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_VersionId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Issues");

            migrationBuilder.CreateTable(
                name: "VersionIssue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionIssue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersionIssue_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionIssue_Versions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VersionIssue_IssueId",
                table: "VersionIssue",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionIssue_VersionId",
                table: "VersionIssue",
                column: "VersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VersionIssue");

            migrationBuilder.AddColumn<Guid>(
                name: "VersionId",
                table: "Issues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_VersionId",
                table: "Issues",
                column: "VersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Versions_VersionId",
                table: "Issues",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id");
        }
    }
}
