#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetGroomingApp.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddUserServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "UserServices",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServices", x => new { x.UserId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_UserServices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a7144e6-1142-47ef-a595-1ca63a774afb", "AQAAAAIAAYagAAAAEHTKR7RwMGNztRJasLf9oUBw/Mv5By3cu686ZEKAhWjIYouTxKJM8eDen3Er/aH27A==", "51ce63df-8569-4d49-8cab-3d39844da0a1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd710d5a-f039-4b20-8652-afdeba1e4474", "AQAAAAIAAYagAAAAECTZY7AjzzqxtwjXE9+D0St65k9ynKqmmBQwaxZnyoEaBGGSYAtoEEws/z5obeEaaw==", "b9fd72db-a5eb-42c3-9e22-1b78fd025cc3" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Duration", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("19780568-cccc-4186-9a7e-eec6067fff1c"), "Bathing with organic shampoo", "00:30:00", "img/service/bath-dog.png", "Bath", 25.00m },
                    { new Guid("57b2b212-fab4-4fce-9bb8-2d6b2f9b6ab5"), "Full body grooming and trim", "00:30:00", "img/service/grooming.png", "Hair Trim", 25.00m },
                    { new Guid("7a4152ef-22ba-4d14-b621-da300be177b2"), "Claw trimming and smoothing", "00:15:00", "img/service/nail-file.png", "Nail Clip", 15.00m },
                    { new Guid("9de5b9d4-4628-43a3-a8b5-6c030479a5d4"), "Carefully cleanning the ears", "00:15:00", "img/service/ear-cleaning-cat.png", "Ear Clean", 10.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_ServiceId",
                table: "UserServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserServices");

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("19780568-cccc-4186-9a7e-eec6067fff1c"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("57b2b212-fab4-4fce-9bb8-2d6b2f9b6ab5"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("7a4152ef-22ba-4d14-b621-da300be177b2"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("9de5b9d4-4628-43a3-a8b5-6c030479a5d4"));

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
    }
}
