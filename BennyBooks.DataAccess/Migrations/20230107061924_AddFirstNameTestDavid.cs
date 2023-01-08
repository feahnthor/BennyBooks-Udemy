using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BennyBooks.DataAccess.Migrations
{
    public partial class AddFirstNameTestDavid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Products");
        }
    }
}
