using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_WepAPI.Migrations
{
    /// <inheritdoc />
    public partial class addApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4503));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4507));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4301));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4315));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4317));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4319));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 52, 58, 165, DateTimeKind.Local).AddTicks(4321));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(4158));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(4153));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(4157));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(3691));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(3712));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(3714));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(3815));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 7, 21, 50, 22, 685, DateTimeKind.Local).AddTicks(3818));
        }
    }
}
