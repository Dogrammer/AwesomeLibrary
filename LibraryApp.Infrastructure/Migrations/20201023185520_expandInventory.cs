using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Infrastructure.Migrations
{
    public partial class expandInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentQuantity",
                table: "BookInventories",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentQuantity",
                table: "BookInventories");
        }
    }
}
