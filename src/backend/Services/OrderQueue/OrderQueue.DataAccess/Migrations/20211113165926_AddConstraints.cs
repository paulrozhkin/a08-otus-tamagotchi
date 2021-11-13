using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderQueue.DataAccess.Migrations
{
    public partial class AddConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KitchenOrders_CreatedDate",
                table: "KitchenOrders",
                column: "CreatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KitchenOrders_CreatedDate",
                table: "KitchenOrders");
        }
    }
}
