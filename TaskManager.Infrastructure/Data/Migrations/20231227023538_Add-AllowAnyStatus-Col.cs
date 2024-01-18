#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowAnyStatusCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelIssues_Versions_VersionId",
                table: "LabelIssues");

            migrationBuilder.DropIndex(
                name: "IX_LabelIssues_VersionId",
                table: "LabelIssues");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "LabelIssues");

            migrationBuilder.AddColumn<bool>(
                name: "AllowAnyStatus",
                table: "Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowAnyStatus",
                table: "Statuses");

            migrationBuilder.AddColumn<Guid>(
                name: "VersionId",
                table: "LabelIssues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabelIssues_VersionId",
                table: "LabelIssues",
                column: "VersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelIssues_Versions_VersionId",
                table: "LabelIssues",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id");
        }
    }
}
