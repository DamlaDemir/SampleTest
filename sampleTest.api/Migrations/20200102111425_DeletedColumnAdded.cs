using Microsoft.EntityFrameworkCore.Migrations;

namespace sampleTest.api.Migrations
{
    public partial class DeletedColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Silindi",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Silindi",
                table: "Users");
        }
    }
}
