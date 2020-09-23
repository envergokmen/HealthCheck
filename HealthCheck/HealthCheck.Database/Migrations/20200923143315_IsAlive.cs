using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCheck.Database.Migrations
{
    public partial class IsAlive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlive",
                table: "TargetApps",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCheck",
                table: "TargetApps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlive",
                table: "TargetApps");

            migrationBuilder.DropColumn(
                name: "LastCheck",
                table: "TargetApps");
        }
    }
}
