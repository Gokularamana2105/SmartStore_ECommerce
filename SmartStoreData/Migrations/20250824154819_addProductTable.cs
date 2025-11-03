using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStoreData.Migrations
{
    /// <inheritdoc />
    public partial class addProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "products",
                newName: "LowPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "HighPrice",
                table: "products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Ratings",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighPrice",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Ratings",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "LowPrice",
                table: "products",
                newName: "Price");
        }
    }
}
