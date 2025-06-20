using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixTripShareNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_SharedWithUserId",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_TripShares_TripId_SharedWithUserId",
                table: "TripShares");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "TripShares",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ChecklistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_OwnerId",
                table: "TripShares",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_TripId",
                table: "TripShares",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_UserId",
                table: "ChecklistItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_users_UserId",
                table: "ChecklistItems",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_users_OwnerId",
                table: "TripShares",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_users_SharedWithUserId",
                table: "TripShares",
                column: "SharedWithUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_users_UserId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_OwnerId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_SharedWithUserId",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_TripShares_OwnerId",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_TripShares_TripId",
                table: "TripShares");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistItems_UserId",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TripShares");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChecklistItems");

            migrationBuilder.CreateIndex(
                name: "IX_TripShares_TripId_SharedWithUserId",
                table: "TripShares",
                columns: new[] { "TripId", "SharedWithUserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_users_SharedWithUserId",
                table: "TripShares",
                column: "SharedWithUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
