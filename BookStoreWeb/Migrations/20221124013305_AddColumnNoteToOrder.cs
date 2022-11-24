using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreWeb.Migrations
{
    public partial class AddColumnNoteToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderHeaders");
        }
    }
}
