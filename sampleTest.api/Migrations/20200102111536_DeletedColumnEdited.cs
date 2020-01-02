using Microsoft.EntityFrameworkCore.Migrations;

namespace sampleTest.api.Migrations
{
    public partial class DeletedColumnEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Silindi",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Silindi",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
