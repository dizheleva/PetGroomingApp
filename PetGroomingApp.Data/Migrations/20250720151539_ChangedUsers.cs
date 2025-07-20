#nullable disable

namespace PetGroomingApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class ChangedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "093ccb78-0b0c-4cfe-bb49-ad20aeec711f", "user1@mail.com", "USER1@MAIL.COM", "USER1@MAIL.COM", "AQAAAAIAAYagAAAAEEW1rCz9KtawBrFEMgz14ykUFxMBd/QHK+L4ZNPNkq6CBd5Sp/OvOEt8i2/rFyvfew==", "fb733b0c-19c4-48d7-9784-6289797ab57e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e8b4a11-4dff-410c-8177-be54e6d9953d", "user2@mail.com", "USER2@MAIL.COM", "USER2@MAIL.COM", "AQAAAAIAAYagAAAAEGDisHiKXTu9SVtlYy3/kUkcdiYeWh3NCFinabTLtlUNpb1fW/IOFhxalQDWgRkZcQ==", "20d67841-680c-4a47-9fe6-a13d442a3a76" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15310e96-4af8-42cf-a017-c9959109ae3f", null, null, "user1", "AQAAAAIAAYagAAAAEIl0pVACFU1uPHmGlRp/SnDizr34grmXocLctC8TUCJXm3HAXz4Y8IWIXOczMzUqlg==", "6488f47e-8092-4ee1-9662-9a38ce78b391" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba4c0966-94b0-4879-997d-35172fd02a32", null, null, "user2", "AQAAAAIAAYagAAAAEAW8ku4BTpxgrft96X80yKABb00zzY6jh9Qps8o4DEV2ezZbDOgIKkM3nDJyyacGDw==", "63d244c2-5d0e-40ff-aac4-c2f98bc4d149" });
        }
    }
}
