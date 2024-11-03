using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_WepAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeSomePropertyToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "villas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "villas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Amenity",
                table: "villas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialDetails",
                table: "villaNumbers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6649));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6439));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6451));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6454));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6456));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 2, 40, 44, 408, DateTimeKind.Local).AddTicks(6458));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Amenity",
                table: "villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SpecialDetails",
                table: "villaNumbers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(251));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(247));

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(33));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(49));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(51));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(55));
        }
    }
}
