using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Menu.Infrastructure.Migrations
{
    public partial class CreateAndUpdateInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Dishes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Dishes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CreatedDate",
                table: "Dishes",
                column: "CreatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dishes_CreatedDate",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Dishes");
        }
    }
}
