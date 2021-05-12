using Microsoft.EntityFrameworkCore.Migrations;

namespace BAI3.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "actualPasswordSchema",
                table: "FragmentalPasswordSchemas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actualPasswordSchema",
                table: "FragmentalPasswordSchemas");
        }
    }
}
