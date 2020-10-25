using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Infrastructure.Migrations
{
    public partial class addQonBookLoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BookLoans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BookLoans");
        }
    }
}
