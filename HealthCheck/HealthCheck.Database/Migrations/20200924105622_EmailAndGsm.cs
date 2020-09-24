using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCheck.Database.Migrations
{
    public partial class EmailAndGsm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gsm",
                table: "Users",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gsm",
                table: "Users");
        }
    }
}
