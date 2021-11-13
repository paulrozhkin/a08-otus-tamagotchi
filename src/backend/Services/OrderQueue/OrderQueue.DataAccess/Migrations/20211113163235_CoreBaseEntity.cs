using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderQueue.DataAccess.Migrations
{
    public partial class CoreBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "KitchenOrders");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "KitchenOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "KitchenOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "KitchenOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "KitchenOrders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "KitchenOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
