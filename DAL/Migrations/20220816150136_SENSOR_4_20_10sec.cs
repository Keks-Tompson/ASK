using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASK.DAL.Migrations
{
    public partial class SENSOR_4_20_10sec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SENSOR_4_20_10sec",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    CO = table.Column<double>(nullable: false),
                    CO2 = table.Column<double>(nullable: false),
                    NO = table.Column<double>(nullable: false),
                    NO2 = table.Column<double>(nullable: false),
                    NOx = table.Column<double>(nullable: false),
                    SO2 = table.Column<double>(nullable: false),
                    Dust = table.Column<double>(nullable: false),
                    CH4 = table.Column<double>(nullable: false),
                    H2S = table.Column<double>(nullable: false),
                    Rezerv_1 = table.Column<double>(nullable: false),
                    Rezerv_2 = table.Column<double>(nullable: false),
                    Rezerv_3 = table.Column<double>(nullable: false),
                    Rezerv_4 = table.Column<double>(nullable: false),
                    Rezerv_5 = table.Column<double>(nullable: false),
                    O2_Wet = table.Column<double>(nullable: false),
                    O2_Dry = table.Column<double>(nullable: false),
                    H2O = table.Column<double>(nullable: false),
                    Pressure = table.Column<double>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    Speed = table.Column<double>(nullable: false),
                    Temperature_KIP = table.Column<double>(nullable: false),
                    Temperature_NOx = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENSOR_4_20_10sec", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SENSOR_4_20_10sec");
        }
    }
}
