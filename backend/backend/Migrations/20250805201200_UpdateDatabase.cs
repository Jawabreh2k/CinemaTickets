using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinema_tickets.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2025, 8, 5, 21, 16, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 5, 19, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2025, 8, 5, 23, 28, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 5, 21, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2025, 8, 6, 22, 32, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 6, 20, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 8, 5, 21, 16, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 5, 19, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 8, 5, 23, 28, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 5, 21, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 8, 6, 22, 32, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 6, 20, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
