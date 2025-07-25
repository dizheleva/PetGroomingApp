#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetGroomingApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class UpdatedGroomerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("9de5b9d4-4628-43a3-a8b5-6c030479a5d4"));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Groomers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Groomer phone number",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Groomer phone number");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Groomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Groomer image URL");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Groomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Groomer job title");

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

            migrationBuilder.InsertData(
                table: "Groomers",
                columns: new[] { "Id", "Description", "FirstName", "ImageUrl", "IsDeleted", "JobTitle", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("5ae6c761-1363-4a23-9965-171c70f935de"), "Experienced groomer with a passion for animals.", "Tom", "img/team/1.png", false, "Groomer, Manager", "Brown", "555-5678" },
                    { new Guid("b2c1d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), "Expert in grooming all breeds of dogs and cats.", "John", "img/team/3.png", false, "Senior Groomer", "Doe", "555-8765" },
                    { new Guid("f4c3e429-0e36-47af-99a2-0c7581a7fc67"), "Loves grooming and caring for pets.", "Lara", "img/team/2.png", false, "Groomer Assistant", "Smith", "555-1234" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Duration", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("61d99a19-c29a-4c38-b2d1-df5f05d3a5e1"), "Professional teeth cleaning for pets", "00:30:00", "img/service/teeth-cleaning.png", "Teeth Cleaning", 30.00m },
                    { new Guid("a1b2c3d4-e5f6-4a8b-9c0d-e1f2a3b4c5d6"), "Carefully cleaning the ears", "00:15:00", "img/service/ear-cleaning-cat.png", "Ear Clean", 10.00m },
                    { new Guid("da876cd4-8c8a-4979-9a53-3d16bc1394fd"), "Includes hair trim, nail clip, ear clean, and bath", "01:30:00", "img/service/full-grooming.png", "Full Grooming Package", 70.00m },
                    { new Guid("f3f8a9ef-6dc0-4c8b-a9b1-9bd250d4e8e6"), "Reduces shedding and improves coat health", "00:45:00", "img/service/deshedding.png", "De-shedding Treatment", 40.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("5ae6c761-1363-4a23-9965-171c70f935de"));

            migrationBuilder.DeleteData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("b2c1d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"));

            migrationBuilder.DeleteData(
                table: "Groomers",
                keyColumn: "Id",
                keyValue: new Guid("f4c3e429-0e36-47af-99a2-0c7581a7fc67"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("61d99a19-c29a-4c38-b2d1-df5f05d3a5e1"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4a8b-9c0d-e1f2a3b4c5d6"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("da876cd4-8c8a-4979-9a53-3d16bc1394fd"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("f3f8a9ef-6dc0-4c8b-a9b1-9bd250d4e8e6"));

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Groomers");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Groomers");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Groomers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "Groomer phone number",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Groomer phone number");

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
                values: new object[] { new Guid("9de5b9d4-4628-43a3-a8b5-6c030479a5d4"), "Carefully cleanning the ears", "00:15:00", "img/service/ear-cleaning-cat.png", "Ear Clean", 10.00m });
        }
    }
}
