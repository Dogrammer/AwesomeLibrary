using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Infrastructure.Migrations
{
    public partial class deletecolloan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Loans_LoanId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_LoanId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LoanId",
                table: "Books",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_LoanId",
                table: "Books",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Loans_LoanId",
                table: "Books",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
