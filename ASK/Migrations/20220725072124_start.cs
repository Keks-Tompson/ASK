using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASK.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCIDENT_LIST",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Accident = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCIDENT_LIST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ACCIDENT_LOG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_accident = table.Column<int>(nullable: false),
                    Date_Begin = table.Column<DateTime>(nullable: false),
                    Time_End = table.Column<DateTime>(nullable: false),
                    is_Error = table.Column<bool>(nullable: false),
                    is_warning = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCIDENT_LOG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AVG_20_MINUTE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Conc_CO = table.Column<double>(nullable: false),
                    Conc_CO2 = table.Column<double>(nullable: false),
                    Conc_NO = table.Column<double>(nullable: false),
                    Conc_NO2 = table.Column<double>(nullable: false),
                    Conc_NOx = table.Column<double>(nullable: false),
                    Conc_SO2 = table.Column<double>(nullable: false),
                    Conc_Dust = table.Column<double>(nullable: false),
                    Conc_CH4 = table.Column<double>(nullable: false),
                    Conc_H2S = table.Column<double>(nullable: false),
                    Conc_D1 = table.Column<double>(nullable: false),
                    Conc_D2 = table.Column<double>(nullable: false),
                    Conc_D3 = table.Column<double>(nullable: false),
                    Conc_D4 = table.Column<double>(nullable: false),
                    Conc_D5 = table.Column<double>(nullable: false),
                    Emis_CO = table.Column<double>(nullable: false),
                    Emis_CO2 = table.Column<double>(nullable: false),
                    Emis_NO = table.Column<double>(nullable: false),
                    Emis_NO2 = table.Column<double>(nullable: false),
                    Emis_NOx = table.Column<double>(nullable: false),
                    Emis_SO2 = table.Column<double>(nullable: false),
                    Emis_CH4 = table.Column<double>(nullable: false),
                    Emis_H2S = table.Column<double>(nullable: false),
                    Emis_Dust = table.Column<double>(nullable: false),
                    Emis_D1 = table.Column<double>(nullable: false),
                    Emis_D2 = table.Column<double>(nullable: false),
                    Emis_D3 = table.Column<double>(nullable: false),
                    Emis_D4 = table.Column<double>(nullable: false),
                    Emis_D5 = table.Column<double>(nullable: false),
                    O2_Wet = table.Column<double>(nullable: false),
                    O2_Dry = table.Column<double>(nullable: false),
                    H2O = table.Column<double>(nullable: false),
                    Pressure = table.Column<double>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    Speed = table.Column<double>(nullable: false),
                    Flow = table.Column<double>(nullable: false),
                    Temperature_KIP = table.Column<double>(nullable: false),
                    Temperature_NOx = table.Column<double>(nullable: false),
                    Mode_ASK = table.Column<int>(nullable: false),
                    PDZ_Fuel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AVG_20_MINUTE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDZ",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberPDZ = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CO_Conc = table.Column<double>(nullable: false),
                    CO2_Conc = table.Column<double>(nullable: false),
                    NO_Conc = table.Column<double>(nullable: false),
                    NO2_Conc = table.Column<double>(nullable: false),
                    NOx_Conc = table.Column<double>(nullable: false),
                    SO2_Conc = table.Column<double>(nullable: false),
                    Dust_Conc = table.Column<double>(nullable: false),
                    CH4_Conc = table.Column<double>(nullable: false),
                    H2S_Conc = table.Column<double>(nullable: false),
                    Add_Conc_1 = table.Column<double>(nullable: false),
                    Add_Conc_2 = table.Column<double>(nullable: false),
                    Add_Conc_3 = table.Column<double>(nullable: false),
                    Add_Conc_4 = table.Column<double>(nullable: false),
                    Add_Conc_5 = table.Column<double>(nullable: false),
                    CO_Emis = table.Column<double>(nullable: false),
                    CO2_Emis = table.Column<double>(nullable: false),
                    NO_Emis = table.Column<double>(nullable: false),
                    NO2_Emis = table.Column<double>(nullable: false),
                    NOx_Emis = table.Column<double>(nullable: false),
                    SO2_Emis = table.Column<double>(nullable: false),
                    Dust_Emis = table.Column<double>(nullable: false),
                    CH4_Emis = table.Column<double>(nullable: false),
                    H2S_Emis = table.Column<double>(nullable: false),
                    Add_Emis_1 = table.Column<double>(nullable: false),
                    Add_Emis_2 = table.Column<double>(nullable: false),
                    Add_Emis_3 = table.Column<double>(nullable: false),
                    Add_Emis_4 = table.Column<double>(nullable: false),
                    Add_Emis_5 = table.Column<double>(nullable: false),
                    Current = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDZ", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCIDENT_LIST");

            migrationBuilder.DropTable(
                name: "ACCIDENT_LOG");

            migrationBuilder.DropTable(
                name: "AVG_20_MINUTE");

            migrationBuilder.DropTable(
                name: "PDZ");
        }
    }
}
