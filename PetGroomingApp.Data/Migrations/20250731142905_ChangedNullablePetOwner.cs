#nullable disable

namespace PetGroomingApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class ChangedNullablePetOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Appointment_Date",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Pets",
                type: "nvarchar(450)",
                nullable: true,
                comment: "Foreign key to the owner of the pet",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Foreign key to the owner of the pet");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Appointment date and time");

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Manager identifier"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Manager's user entity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manager_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Manager in the system");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "29aff6d7-a122-4187-ab02-11cf2b74fc0e", "AQAAAAIAAYagAAAAEPBVp36O46lYeSp8OXQs8CfSDMcYTk7uPFRWMPVwjPo3ZfQ3H2klaXM6pt5aLQCleQ==", "8fb9c064-4c3f-4cbd-b4f1-06d6d054730e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc1f04f5-1e0a-425f-9b3a-4d68c786643d", "AQAAAAIAAYagAAAAEPWBZEQjOVu+w3LcRO0gpa8xdJLY0qvEzTWqILpzK+HkMtWnyN+/r+OwQCm+RtcC7g==", "c355e629-c224-4fab-b4a5-e0998d07f59a" });

            migrationBuilder.CreateIndex(
                name: "Appointment_Time",
                table: "Appointments",
                column: "AppointmentTime");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserId",
                table: "Manager",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropIndex(
                name: "Appointment_Time",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentTime",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Pets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                comment: "Foreign key to the owner of the pet",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComment: "Foreign key to the owner of the pet");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Appointment date");

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

            migrationBuilder.CreateIndex(
                name: "Appointment_Date",
                table: "Appointments",
                column: "Date");
        }
    }
}
