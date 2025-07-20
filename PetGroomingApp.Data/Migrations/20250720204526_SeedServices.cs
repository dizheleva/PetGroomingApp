#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetGroomingApp.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d484c50c-d049-4f42-9d9a-bfb333cea96a", "AQAAAAIAAYagAAAAECNowpWV258mMT+oGBLSoBtEAQSR7GgBmFQo5KqZ4Ex2fjXjHinHjYCGLGM7w6noxQ==", "80799a95-31ce-4ff0-a2f8-c435340fff43" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bc86046a-633f-46c2-9a73-e9d473dfe82b", "AQAAAAIAAYagAAAAEP6ZW7oFVboZho/MMcvT4QXtAXNg6XKCP/CRFXbdFdZCpZz1n4j83owpxqZtQuaEmg==", "d1a79ded-2f40-4805-824d-253e56cb0702" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Duration", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("309f0da9-4be9-46fb-a109-568daa4a1c31"), "Bathing with organic shampoo.Drying and perfuming", "00:30:00", "img/service/service_icon_2.png", "Bath", 25.00m },
                    { new Guid("5f2e2ab4-a31b-4487-b1a4-53ec71c75dce"), "Carefully cleanning the ears", "00:15:00", "img/service/service_icon_1.png", "Ear Clean", 10.00m },
                    { new Guid("6005c295-4442-4521-b52f-98816883ce4d"), "Claw trimming and smoothing", "00:15:00", "img/service/service_icon_3.png", "Nail Clip", 15.00m },
                    { new Guid("a5894724-6c79-4672-a3ed-912b76326cf6"), "Full body grooming and trim", "00:30:00", "img/service/service_icon_2.png", "Hair Trim", 25.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("309f0da9-4be9-46fb-a109-568daa4a1c31"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("5f2e2ab4-a31b-4487-b1a4-53ec71c75dce"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("6005c295-4442-4521-b52f-98816883ce4d"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("a5894724-6c79-4672-a3ed-912b76326cf6"));

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
    }
}
