using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCheck.Database.Migrations
{
    public partial class Intervals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationPreference",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntervalType",
                table: "TargetApps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntervalValue",
                table: "TargetApps",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationPreference",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IntervalType",
                table: "TargetApps");

            migrationBuilder.DropColumn(
                name: "IntervalValue",
                table: "TargetApps");
        }
    }
}
