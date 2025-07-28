#nullable disable

namespace PetGroomingApp.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class UpdatePetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Pet gender");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b5e473f6-1fdb-4051-9068-cbae4b5bae02", "AQAAAAIAAYagAAAAEIO+s65Iwhn7uG5B99zFt8u17WrgcZqed4xrAo7bv+01/J2w7UbHHzNo0O1LLNHpJQ==", "99b8fd35-962e-4a18-94da-d623f5007e3c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e95b34e-fa33-4090-bb04-eb52eb52e3f3", "AQAAAAIAAYagAAAAEAn+jR0E3dLn2X3l1jYHcd6RrV7lVQYSxLmF0cTzbH6gZWicz1vDAHmrZfmMiGN54w==", "e7f65e22-a8e0-4b09-ac44-fb77b2bf6403" });

            migrationBuilder.UpdateData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("5ae6c761-1363-4a23-9965-171c70f935de"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Lara", "Smith" });

            migrationBuilder.UpdateData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("b2c1d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Sara", "Croft" });

            migrationBuilder.UpdateData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("f4c3e429-0e36-47af-99a2-0c7581a7fc67"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Tom", "Brown" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Pets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a2db80b-31bc-4846-ba20-6c0c94e60276", "AQAAAAIAAYagAAAAECTALnqwCthsYmeQi5G0MQeSpDxwknzt7BVkCfdZak5ilU9xxOWgGS370TOlAs1URA==", "d88693f4-66e4-4d6d-92c3-a4512e826c65" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0df0559-5faa-4919-8661-4302a577c493", "AQAAAAIAAYagAAAAEJF8OniEPdc4f3k951bGUBqTIJVjCvpoHoRppcmN1tjqI0GX1TWyDKgfY1MaYtlM/A==", "3c5911ec-bf90-41c1-ad9e-67dc00f88051" });

            migrationBuilder.UpdateData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("5ae6c761-1363-4a23-9965-171c70f935de"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Tom", "Brown" });

            migrationBuilder.UpdateData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("b2c1d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "John", "Doe" });

            migrationBuilder.UpdateData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("f4c3e429-0e36-47af-99a2-0c7581a7fc67"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Lara", "Smith" });
        }
    }
}
