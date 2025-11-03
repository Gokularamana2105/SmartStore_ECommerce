using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStoreData.Migrations
{
    /// <inheritdoc />
    public partial class addUpdatedOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "orders",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "orders");
        }
    }
}
