using Microsoft.EntityFrameworkCore.Migrations;

namespace ASK.Migrations
{
    public partial class Alarmlist_Is_Stop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_warning",
                table: "ACCIDENT_LIST");

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "ACCIDENT_LOG",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "ACCIDENT_LOG");

            migrationBuilder.AddColumn<bool>(
                name: "is_warning",
                table: "ACCIDENT_LIST",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
