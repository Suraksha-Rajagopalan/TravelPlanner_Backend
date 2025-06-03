using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySetupV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDetails_Trips_TripId",
                table: "BudgetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_roleclaims_role_RoleId",
                table: "roleclaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_user_UserId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_userclaims_user_UserId",
                table: "userclaims");

            migrationBuilder.DropForeignKey(
                name: "FK_userlogins_user_UserId",
                table: "userlogins");

            migrationBuilder.DropForeignKey(
                name: "FK_userroles_role_RoleId",
                table: "userroles");

            migrationBuilder.DropForeignKey(
                name: "FK_userroles_user_UserId",
                table: "userroles");

            migrationBuilder.DropForeignKey(
                name: "FK_usertokens_user_UserId",
                table: "usertokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetDetails",
                table: "BudgetDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usertokens",
                table: "usertokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userroles",
                table: "userroles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userlogins",
                table: "userlogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userclaims",
                table: "userclaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roleclaims",
                table: "roleclaims");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "trips");

            migrationBuilder.RenameTable(
                name: "BudgetDetails",
                newName: "budgetdetails");

            migrationBuilder.RenameTable(
                name: "usertokens",
                newName: "user_tokens");

            migrationBuilder.RenameTable(
                name: "userroles",
                newName: "user_roles");

            migrationBuilder.RenameTable(
                name: "userlogins",
                newName: "user_logins");

            migrationBuilder.RenameTable(
                name: "userclaims",
                newName: "user_claims");

            migrationBuilder.RenameTable(
                name: "roleclaims",
                newName: "role_claims");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_UserId",
                table: "trips",
                newName: "IX_trips_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetDetails_TripId",
                table: "budgetdetails",
                newName: "IX_budgetdetails_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_userroles_RoleId",
                table: "user_roles",
                newName: "IX_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_userlogins_UserId",
                table: "user_logins",
                newName: "IX_user_logins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userclaims_UserId",
                table: "user_claims",
                newName: "IX_user_claims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_roleclaims_RoleId",
                table: "role_claims",
                newName: "IX_role_claims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_trips",
                table: "trips",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_budgetdetails",
                table: "budgetdetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_tokens",
                table: "user_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_logins",
                table: "user_logins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_claims",
                table: "user_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_claims",
                table: "role_claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_budgetdetails_trips_TripId",
                table: "budgetdetails",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_claims_role_RoleId",
                table: "role_claims",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_trips_user_UserId",
                table: "trips",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_user_UserId",
                table: "user_claims",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_user_UserId",
                table: "user_logins",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_role_RoleId",
                table: "user_roles",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_user_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_user_UserId",
                table: "user_tokens",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_budgetdetails_trips_TripId",
                table: "budgetdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_role_claims_role_RoleId",
                table: "role_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_trips_user_UserId",
                table: "trips");

            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_user_UserId",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_user_UserId",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_role_RoleId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_user_UserId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_user_UserId",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_trips",
                table: "trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_budgetdetails",
                table: "budgetdetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_tokens",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_logins",
                table: "user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_claims",
                table: "user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_claims",
                table: "role_claims");

            migrationBuilder.RenameTable(
                name: "trips",
                newName: "Trips");

            migrationBuilder.RenameTable(
                name: "budgetdetails",
                newName: "BudgetDetails");

            migrationBuilder.RenameTable(
                name: "user_tokens",
                newName: "usertokens");

            migrationBuilder.RenameTable(
                name: "user_roles",
                newName: "userroles");

            migrationBuilder.RenameTable(
                name: "user_logins",
                newName: "userlogins");

            migrationBuilder.RenameTable(
                name: "user_claims",
                newName: "userclaims");

            migrationBuilder.RenameTable(
                name: "role_claims",
                newName: "roleclaims");

            migrationBuilder.RenameIndex(
                name: "IX_trips_UserId",
                table: "Trips",
                newName: "IX_Trips_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_budgetdetails_TripId",
                table: "BudgetDetails",
                newName: "IX_BudgetDetails_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_RoleId",
                table: "userroles",
                newName: "IX_userroles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_user_logins_UserId",
                table: "userlogins",
                newName: "IX_userlogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_user_claims_UserId",
                table: "userclaims",
                newName: "IX_userclaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_role_claims_RoleId",
                table: "roleclaims",
                newName: "IX_roleclaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetDetails",
                table: "BudgetDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usertokens",
                table: "usertokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_userroles",
                table: "userroles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_userlogins",
                table: "userlogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_userclaims",
                table: "userclaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roleclaims",
                table: "roleclaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDetails_Trips_TripId",
                table: "BudgetDetails",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_roleclaims_role_RoleId",
                table: "roleclaims",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_user_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userclaims_user_UserId",
                table: "userclaims",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userlogins_user_UserId",
                table: "userlogins",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userroles_role_RoleId",
                table: "userroles",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userroles_user_UserId",
                table: "userroles",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_usertokens_user_UserId",
                table: "usertokens",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
