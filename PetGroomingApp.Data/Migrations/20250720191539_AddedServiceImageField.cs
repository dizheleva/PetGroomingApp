#nullable disable

namespace PetGroomingApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddedServiceImageField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Service image");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Services");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "093ccb78-0b0c-4cfe-bb49-ad20aeec711f", "AQAAAAIAAYagAAAAEEW1rCz9KtawBrFEMgz14ykUFxMBd/QHK+L4ZNPNkq6CBd5Sp/OvOEt8i2/rFyvfew==", "fb733b0c-19c4-48d7-9784-6289797ab57e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e8b4a11-4dff-410c-8177-be54e6d9953d", "AQAAAAIAAYagAAAAEGDisHiKXTu9SVtlYy3/kUkcdiYeWh3NCFinabTLtlUNpb1fW/IOFhxalQDWgRkZcQ==", "20d67841-680c-4a47-9fe6-a13d442a3a76" });
        }
    }
}
