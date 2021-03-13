using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeterReaderWeb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReadingDate",
                value: new DateTime(2021, 3, 13, 10, 31, 25, 92, DateTimeKind.Local).AddTicks(7332));

            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReadingDate",
                value: new DateTime(2021, 3, 13, 10, 31, 25, 94, DateTimeKind.Local).AddTicks(197));

            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReadingDate",
                value: new DateTime(2021, 3, 13, 10, 31, 25, 94, DateTimeKind.Local).AddTicks(234));

            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReadingDate",
                value: new DateTime(2021, 3, 13, 10, 31, 25, 94, DateTimeKind.Local).AddTicks(238));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReadingDate",
                value: new DateTime(2019, 9, 24, 23, 38, 57, 515, DateTimeKind.Local).AddTicks(4150));

            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReadingDate",
                value: new DateTime(2019, 9, 24, 23, 38, 57, 518, DateTimeKind.Local).AddTicks(8672));

            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReadingDate",
                value: new DateTime(2019, 9, 24, 23, 38, 57, 518, DateTimeKind.Local).AddTicks(8719));

            migrationBuilder.UpdateData(
                table: "Readings",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReadingDate",
                value: new DateTime(2019, 9, 24, 23, 38, 57, 518, DateTimeKind.Local).AddTicks(8724));
        }
    }
}
