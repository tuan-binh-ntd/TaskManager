using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityIdCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Issues");

            migrationBuilder.AddColumn<Guid>(
                name: "PriorityId",
                table: "Issues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_PriorityId",
                table: "Issues",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Priorities_PriorityId",
                table: "Issues",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Priorities_PriorityId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_PriorityId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "Issues");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
