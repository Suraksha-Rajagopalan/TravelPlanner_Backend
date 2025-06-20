using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddItineraryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_trips_TripId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewText",
                table: "reviews",
                newName: "review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_TripId_UserId",
                table: "reviews",
                newName: "IX_reviews_TripId_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reviews",
                table: "reviews",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "itinerary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Activity = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itinerary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_itinerary_trips_TripId",
                        column: x => x.TripId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_itinerary_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId",
                table: "reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_itinerary_TripId",
                table: "itinerary",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_itinerary_UserId",
                table: "itinerary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_trips_TripId",
                table: "reviews",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_trips_TripId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews");

            migrationBuilder.DropTable(
                name: "itinerary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reviews",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_UserId",
                table: "reviews");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "Reviews");

            migrationBuilder.RenameColumn(
                name: "review",
                table: "Reviews",
                newName: "ReviewText");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_TripId_UserId",
                table: "Reviews",
                newName: "IX_Reviews_TripId_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_trips_TripId",
                table: "Reviews",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
