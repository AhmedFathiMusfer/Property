using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_WepAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddForgetPassworedToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fogetPassworeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpieresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Valid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fogetPassworeds", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 5, 18, 42, 44, 485, DateTimeKind.Local).AddTicks(3559));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fogetPassworeds");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 2, 17, 30, 47, 234, DateTimeKind.Local).AddTicks(3904));
        }
    }
}
