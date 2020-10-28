using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Infrastructure.Migrations
{
    public partial class overdue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalOverdue",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Overdue",
                table: "Loans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalOverdue",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Overdue",
                table: "Loans");
        }
    }
}
