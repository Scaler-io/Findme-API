using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.DataAccess.Migrations
{
    public partial class EntityTableNamesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_UserProfile_UserProfileId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_UserProfile_UserProfileId",
                table: "UserAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Users_AppUserId",
                table: "UserProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress");

            migrationBuilder.RenameTable(
                name: "UserProfile",
                newName: "Profiles");

            migrationBuilder.RenameTable(
                name: "UserAddress",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfile_AppUserId",
                table: "Profiles",
                newName: "IX_Profiles_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddress_UserProfileId",
                table: "Addresses",
                newName: "IX_Addresses_UserProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Profiles_UserProfileId",
                table: "Addresses",
                column: "UserProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Profiles_UserProfileId",
                table: "Photos",
                column: "UserProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Users_AppUserId",
                table: "Profiles",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Profiles_UserProfileId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Profiles_UserProfileId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Users_AppUserId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Profiles",
                newName: "UserProfile");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "UserAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_AppUserId",
                table: "UserProfile",
                newName: "IX_UserProfile_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserProfileId",
                table: "UserAddress",
                newName: "IX_UserAddress_UserProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_UserProfile_UserProfileId",
                table: "Photos",
                column: "UserProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_UserProfile_UserProfileId",
                table: "UserAddress",
                column: "UserProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Users_AppUserId",
                table: "UserProfile",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
