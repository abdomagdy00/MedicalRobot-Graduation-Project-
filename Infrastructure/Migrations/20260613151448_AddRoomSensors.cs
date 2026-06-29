using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomSensors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "RoomHumidity",
                table: "MedicalRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RoomTemperature",
                table: "MedicalRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RoomHumidity", "RoomTemperature" },
                values: new object[] { 0f, 0f });

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RoomHumidity", "RoomTemperature" },
                values: new object[] { 0f, 0f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomHumidity",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "RoomTemperature",
                table: "MedicalRecords");
        }
    }
}
