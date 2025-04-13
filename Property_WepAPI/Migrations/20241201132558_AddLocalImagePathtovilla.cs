using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_WepAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalImagePathtovilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<string>(
                name: "LocalImagePath",
                table: "villas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LocalImagePath" },
                values: new object[] { new DateTime(2024, 12, 1, 16, 25, 58, 533, DateTimeKind.Local).AddTicks(6086), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LocalImagePath" },
                values: new object[] { new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LocalImagePath" },
                values: new object[] { new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "LocalImagePath" },
                values: new object[] { new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "LocalImagePath" },
                values: new object[] { new DateTime(2004, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalImagePath",
                table: "villas");

           

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
    }
}
