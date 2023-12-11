using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManyCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentLead",
                table: "NotificationIssueEvents");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "NotificationIssueEvents");

            migrationBuilder.DropColumn(
                name: "SingleUser",
                table: "NotificationIssueEvents");

            migrationBuilder.DropColumn(
                name: "Team",
                table: "NotificationIssueEvents");

            migrationBuilder.RenameColumn(
                name: "CurrentUser",
                table: "NotificationIssueEvents",
                newName: "ProjectLead");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectLead",
                table: "NotificationIssueEvents",
                newName: "CurrentUser");

            migrationBuilder.AddColumn<bool>(
                name: "ComponentLead",
                table: "NotificationIssueEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "NotificationIssueEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SingleUser",
                table: "NotificationIssueEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Team",
                table: "NotificationIssueEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
