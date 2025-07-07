using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLastLoginDateToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Users_UserId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Users_UserId1",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_TripShares_TripId",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_TripShares_UserId",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_TripShares_UserId1",
                table: "TripShares");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TripShares");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TripShares");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_TripId_SharedWithUserId",
                table: "TripShares",
                columns: new[] { "TripId", "SharedWithUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TripShares_TripId_SharedWithUserId",
                table: "TripShares");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TripShares",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TripShares",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_TripId",
                table: "TripShares",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_UserId",
                table: "TripShares",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_UserId1",
                table: "TripShares",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_Users_UserId",
                table: "TripShares",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_Users_UserId1",
                table: "TripShares",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
