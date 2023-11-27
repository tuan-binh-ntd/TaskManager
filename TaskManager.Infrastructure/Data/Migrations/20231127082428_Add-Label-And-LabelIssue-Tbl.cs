using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLabelAndLabelIssueTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VersionIssue_Issues_IssueId",
                table: "VersionIssue");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionIssue_Versions_VersionId",
                table: "VersionIssue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VersionIssue",
                table: "VersionIssue");

            migrationBuilder.RenameTable(
                name: "VersionIssue",
                newName: "VersionIssues");

            migrationBuilder.RenameIndex(
                name: "IX_VersionIssue_VersionId",
                table: "VersionIssues",
                newName: "IX_VersionIssues_VersionId");

            migrationBuilder.RenameIndex(
                name: "IX_VersionIssue_IssueId",
                table: "VersionIssues",
                newName: "IX_VersionIssues_IssueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VersionIssues",
                table: "VersionIssues",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabelIssues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabelIssues_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelIssues_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelIssues_Versions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Versions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelIssues_IssueId",
                table: "LabelIssues",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelIssues_LabelId",
                table: "LabelIssues",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelIssues_VersionId",
                table: "LabelIssues",
                column: "VersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VersionIssues_Issues_IssueId",
                table: "VersionIssues",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionIssues_Versions_VersionId",
                table: "VersionIssues",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VersionIssues_Issues_IssueId",
                table: "VersionIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionIssues_Versions_VersionId",
                table: "VersionIssues");

            migrationBuilder.DropTable(
                name: "LabelIssues");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VersionIssues",
                table: "VersionIssues");

            migrationBuilder.RenameTable(
                name: "VersionIssues",
                newName: "VersionIssue");

            migrationBuilder.RenameIndex(
                name: "IX_VersionIssues_VersionId",
                table: "VersionIssue",
                newName: "IX_VersionIssue_VersionId");

            migrationBuilder.RenameIndex(
                name: "IX_VersionIssues_IssueId",
                table: "VersionIssue",
                newName: "IX_VersionIssue_IssueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VersionIssue",
                table: "VersionIssue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VersionIssue_Issues_IssueId",
                table: "VersionIssue",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionIssue_Versions_VersionId",
                table: "VersionIssue",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
