using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MedicineDrawers",
                columns: new[] { "Id", "CommandChar", "DrawerNumber", "DrawerStatus", "IsOpened", "PatientId" },
                values: new object[] { 3, "O3", 3, 0, false, null });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Age", "AssignedDrawerId", "FaceId", "FullName", "Gender" },
                values: new object[,]
                {
                    { 1, 45, 1, 1001, "Mohamed Khaled", 1 },
                    { 2, 30, 2, 1002, "Sara Ahmed", 2 }
                });

            migrationBuilder.InsertData(
                table: "MedicalRecords",
                columns: new[] { "Id", "CapturedAt", "HeartRate", "PatientId", "SpO2", "Temperature" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 72, 1, 95, 36.8f },
                    { 2, new DateTime(2026, 5, 8, 22, 0, 0, 0, DateTimeKind.Unspecified), 80, 1, 92, 37.5f }
                });

            migrationBuilder.InsertData(
                table: "MedicineDrawers",
                columns: new[] { "Id", "CommandChar", "DrawerNumber", "DrawerStatus", "IsOpened", "PatientId" },
                values: new object[,]
                {
                    { 1, "O1", 1, 0, false, 1 },
                    { 2, "O2", 2, 1, true, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicineDrawers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicineDrawers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicineDrawers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
