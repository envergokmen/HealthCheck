using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCheck.Database.Migrations
{
    public partial class EmailAndGsmRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gsm",
                table: "Users",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gsm",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 90);
        }
    }
}
