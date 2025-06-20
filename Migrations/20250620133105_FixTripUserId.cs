using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixTripUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_budgetdetails_trips_TripId",
                table: "budgetdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_trips_TripId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_users_UserId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_trips_TripId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryItems_trips_TripId",
                table: "ItineraryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_trips_TripId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_trips_users_UserId",
                table: "trips");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_trips_TripId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_OwnerId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_SharedWithUserId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_UserId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_users_UserId1",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_users_UserId",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_users_UserId",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_UserId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_users_UserId",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_trips",
                table: "trips");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "trips",
                newName: "Trips");

            migrationBuilder.RenameIndex(
                name: "IX_trips_UserId",
                table: "Trips",
                newName: "IX_Trips_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_budgetdetails_Trips_TripId",
                table: "budgetdetails",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_Trips_TripId",
                table: "ChecklistItems",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_Users_UserId",
                table: "ChecklistItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Trips_TripId",
                table: "Expenses",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryItems_Trips_TripId",
                table: "ItineraryItems",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_Trips_TripId",
                table: "reviews",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_Users_UserId",
                table: "reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_Trips_TripId",
                table: "TripShares",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_Users_OwnerId",
                table: "TripShares",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_Users_SharedWithUserId",
                table: "TripShares",
                column: "SharedWithUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_Users_UserId",
                table: "user_claims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_Users_UserId",
                table: "user_logins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_Users_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_Users_UserId",
                table: "user_tokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_budgetdetails_Trips_TripId",
                table: "budgetdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_Trips_TripId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_Users_UserId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Trips_TripId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryItems_Trips_TripId",
                table: "ItineraryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_Trips_TripId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_Users_UserId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Trips_TripId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Users_OwnerId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Users_SharedWithUserId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Users_UserId",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_TripShares_Users_UserId1",
                table: "TripShares");

            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_Users_UserId",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_Users_UserId",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_Users_UserId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_Users_UserId",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "trips");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_UserId",
                table: "trips",
                newName: "IX_trips_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_trips",
                table: "trips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_budgetdetails_trips_TripId",
                table: "budgetdetails",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_trips_TripId",
                table: "ChecklistItems",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_users_UserId",
                table: "ChecklistItems",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_trips_TripId",
                table: "Expenses",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryItems_trips_TripId",
                table: "ItineraryItems",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_trips_users_UserId",
                table: "trips",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_trips_TripId",
                table: "TripShares",
                column: "TripId",
                principalTable: "trips",
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

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_users_UserId",
                table: "TripShares",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripShares_users_UserId1",
                table: "TripShares",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_users_UserId",
                table: "user_claims",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_users_UserId",
                table: "user_logins",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_users_UserId",
                table: "user_tokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
