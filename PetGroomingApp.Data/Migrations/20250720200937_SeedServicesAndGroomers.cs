#nullable disable

namespace PetGroomingApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedServicesAndGroomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "279a2b8d-622d-4317-94ea-f19f4fe08786", "AQAAAAIAAYagAAAAEDzKb80L+Lw676XkTeRcuRBjrdU+bWp2J6lOM3YQQJOBjA4elyR8JCVtgnG10NFfbw==", "415a1f9d-3288-44f3-a17b-5539f79a7427" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0967bd24-5e5c-4713-9c6e-68045a91dbcb", "AQAAAAIAAYagAAAAEEm0uEBzkBUJO0AAdNN4W6NtnpTFrvuo9tt0e53qhq516RZklAOv7m+DEAe4Ekb2Hg==", "b3548c66-504f-4723-9aa6-2c082f2dec90" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7d173322-11ff-428a-9192-c7626a1cb36c", "AQAAAAIAAYagAAAAEGziWAyBFZzcoD7uZyd1BJIKYjQIGPSVWTWuim46z5cV/ulKwO+nnrF+8iOYTgyK/A==", "b0439ed4-782d-4651-8214-dd6ef2141be6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e3b85f19-08b7-4bfd-b1e0-38360b240c28", "AQAAAAIAAYagAAAAEOUe4odL74hJGDoWkDpEdgRoBlsoGuFrzAU6vCshm4vKMPmh0vXKMOLLnN8mIXb/lg==", "43122cbf-0951-41dc-b08a-64bbcb01cf88" });
        }
    }
}
