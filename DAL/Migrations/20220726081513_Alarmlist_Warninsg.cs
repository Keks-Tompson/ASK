using Microsoft.EntityFrameworkCore.Migrations;

namespace ASK.DAL.Migrations
{
    public partial class Alarmlist_Warninsg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_Error",
                table: "ACCIDENT_LOG");

            migrationBuilder.DropColumn(
                name: "is_warning",
                table: "ACCIDENT_LOG");

            migrationBuilder.AddColumn<bool>(
                name: "is_Error",
                table: "ACCIDENT_LIST",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_warning",
                table: "ACCIDENT_LIST",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_Error",
                table: "ACCIDENT_LIST");

            migrationBuilder.DropColumn(
                name: "is_warning",
                table: "ACCIDENT_LIST");

            migrationBuilder.AddColumn<bool>(
                name: "is_Error",
                table: "ACCIDENT_LOG",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_warning",
                table: "ACCIDENT_LOG",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
