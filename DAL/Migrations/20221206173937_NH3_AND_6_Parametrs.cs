using Microsoft.EntityFrameworkCore.Migrations;

namespace ASK.DAL.Migrations
{
    public partial class NH3_AND_6_Parametrs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "NH3",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "O2_PGS",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "O2_Room",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pressure_KIP",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature_PGS",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature_Point_Dew",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature_Room",
                table: "SENSOR_4_20_10sec",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NH3_Conc",
                table: "PDZ",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NH3_Emis",
                table: "PDZ",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Conc_NH3",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Emis_NH3",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "O2_PGS",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "O2_Room",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pressure_KIP",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature_PGS",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature_Point_Dew",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature_Room",
                table: "AVG_20_MINUTE",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NH3",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "O2_PGS",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "O2_Room",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "Pressure_KIP",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "Temperature_PGS",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "Temperature_Point_Dew",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "Temperature_Room",
                table: "SENSOR_4_20_10sec");

            migrationBuilder.DropColumn(
                name: "NH3_Conc",
                table: "PDZ");

            migrationBuilder.DropColumn(
                name: "NH3_Emis",
                table: "PDZ");

            migrationBuilder.DropColumn(
                name: "Conc_NH3",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "Emis_NH3",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "O2_PGS",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "O2_Room",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "Pressure_KIP",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "Temperature_PGS",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "Temperature_Point_Dew",
                table: "AVG_20_MINUTE");

            migrationBuilder.DropColumn(
                name: "Temperature_Room",
                table: "AVG_20_MINUTE");
        }
    }
}
