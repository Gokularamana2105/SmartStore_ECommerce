using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStoreData.Migrations
{
    /// <inheritdoc />
    public partial class addApplicationUserUpdatedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isValid",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isValid",
                table: "AspNetUsers");
        }
    }
}
