using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_WepAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "refreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtTokenId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Refresh_Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Valid = table.Column<bool>(type: "bit", nullable: false),
                    ExpieresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 2, 17, 30, 47, 234, DateTimeKind.Local).AddTicks(3904));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refreshTokens");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 1, 16, 25, 58, 533, DateTimeKind.Local).AddTicks(6086));
        }
    }
}
