#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectIdFKCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "IssueTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypes_ProjectId",
                table: "IssueTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueTypes_Projects_ProjectId",
                table: "IssueTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueTypes_Projects_ProjectId",
                table: "IssueTypes");

            migrationBuilder.DropIndex(
                name: "IX_IssueTypes_ProjectId",
                table: "IssueTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "IssueTypes");
        }
    }
}
