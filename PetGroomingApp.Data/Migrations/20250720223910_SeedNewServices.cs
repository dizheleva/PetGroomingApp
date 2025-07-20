#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetGroomingApp.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedNewServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "734146c6-905d-488a-9c7a-909d1868c5f7", "AQAAAAIAAYagAAAAEG80iWSKXMc5Ts7/jyk4vPxU3evD0F8C5D/tAE9QrbCDTjLAR4aEBZCGocum1UuPyw==", "3ccc37f0-21fa-4a2d-ae99-6b01c46da615" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8943f39c-9693-418d-b38c-1520b1472eb4", "AQAAAAIAAYagAAAAEP7uatgAc13pqLFNxQn8REtRB0uWOGvpCg9n83mJkRTte8KpKyt7FbsCpdCYu2BUjg==", "958ac02f-ae2c-48b5-931b-e7d3c59fd461" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Duration", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("51a30835-1f37-461a-9d72-af5090f39d26"), "Bathing with organic shampoo.Drying and perfuming", "00:30:00", "img/service/bath-dog.png", "Bath", 25.00m },
                    { new Guid("8dba4287-5e2b-416d-801d-4078f07eed4c"), "Carefully cleanning the ears", "00:15:00", "img/service/ear-cleaning-cat.png", "Ear Clean", 10.00m },
                    { new Guid("a3ea0072-20a5-4c6e-9ee8-b3cf9b90fde1"), "Claw trimming and smoothing", "00:15:00", "img/service/nail-file.png", "Nail Clip", 15.00m },
                    { new Guid("e0ff5435-515d-4105-b8b9-e9863f9ea48a"), "Full body grooming and trim", "00:30:00", "img/service/grooming.png", "Hair Trim", 25.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("51a30835-1f37-461a-9d72-af5090f39d26"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("8dba4287-5e2b-416d-801d-4078f07eed4c"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("a3ea0072-20a5-4c6e-9ee8-b3cf9b90fde1"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("e0ff5435-515d-4105-b8b9-e9863f9ea48a"));

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
    }
}
