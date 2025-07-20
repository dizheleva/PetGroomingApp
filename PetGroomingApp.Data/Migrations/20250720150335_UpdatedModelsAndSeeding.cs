#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetGroomingApp.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class UpdatedModelsAndSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Service duration",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldComment: "Service duration");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", 0, "15310e96-4af8-42cf-a017-c9959109ae3f", null, false, false, null, null, "user1", "AQAAAAIAAYagAAAAEIl0pVACFU1uPHmGlRp/SnDizr34grmXocLctC8TUCJXm3HAXz4Y8IWIXOczMzUqlg==", null, false, "6488f47e-8092-4ee1-9662-9a38ce78b391", false, "user1@mail.com" },
                    { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "ba4c0966-94b0-4879-997d-35172fd02a32", null, false, false, null, null, "user2", "AQAAAAIAAYagAAAAEAW8ku4BTpxgrft96X80yKABb00zzY6jh9Qps8o4DEV2ezZbDOgIKkM3nDJyyacGDw==", null, false, "63d244c2-5d0e-40ff-aac4-c2f98bc4d149", false, "user2@mail.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Services",
                type: "time",
                nullable: false,
                comment: "Service duration",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Service duration");
        }
    }
}
