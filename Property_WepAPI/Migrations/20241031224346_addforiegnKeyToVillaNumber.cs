using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Property_WepAPI.Migrations
{
    /// <inheritdoc />
    public partial class addforiegnKeyToVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

           

            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "villaNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                columns: new[] { "CreatedDate", "VillaId" },
                values: new object[] { new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(251), 1 });

            migrationBuilder.InsertData(
                table: "villaNumbers",
                columns: new[] { "VillaNo", "CreatedDate", "SpecialDetails", "UpdateDate", "VillaId" },
                values: new object[,]
                {
                    { 4, new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(247), "Bage", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, new DateTime(2024, 11, 1, 1, 43, 45, 889, DateTimeKind.Local).AddTicks(250), "Smali", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_villaNumbers_VillaId",
                table: "villaNumbers",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_villaNumbers_villas_VillaId",
                table: "villaNumbers",
                column: "VillaId",
                principalTable: "villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_villaNumbers_villas_VillaId",
                table: "villaNumbers");

            migrationBuilder.DropIndex(
                name: "IX_villaNumbers_VillaId",
                table: "villaNumbers");

            migrationBuilder.DeleteData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "villaNumbers");

            migrationBuilder.UpdateData(
                table: "villaNumbers",
                keyColumn: "VillaNo",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4570));

            migrationBuilder.InsertData(
                table: "villaNumbers",
                columns: new[] { "VillaNo", "CreatedDate", "SpecialDetails", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4568), "Bage", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4569), "Smali", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4419));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4434));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4436));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4438));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 1, 11, 28, 487, DateTimeKind.Local).AddTicks(4439));
        }
    }
}
