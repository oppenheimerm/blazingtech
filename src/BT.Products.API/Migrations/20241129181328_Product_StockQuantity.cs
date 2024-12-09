using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BT.Products.API.Migrations
{
    /// <inheritdoc />
    public partial class Product_StockQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Product",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Product");
        }
    }
}
