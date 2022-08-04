﻿// <auto-generated />
using System;
using ASK.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASK.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220725072124_start")]
    partial class start
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ASK.Models.ACCIDENT_LIST", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Accident")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ACCIDENT_LIST");
                });

            modelBuilder.Entity("ASK.Models.ACCIDENT_LOG", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date_Begin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Time_End")
                        .HasColumnType("datetime2");

                    b.Property<int>("id_accident")
                        .HasColumnType("int");

                    b.Property<bool>("is_Error")
                        .HasColumnType("bit");

                    b.Property<bool>("is_warning")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ACCIDENT_LOG");
                });

            modelBuilder.Entity("ASK.Models.AVG_20_MINUTES", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Conc_CH4")
                        .HasColumnType("float");

                    b.Property<double>("Conc_CO")
                        .HasColumnType("float");

                    b.Property<double>("Conc_CO2")
                        .HasColumnType("float");

                    b.Property<double>("Conc_D1")
                        .HasColumnType("float");

                    b.Property<double>("Conc_D2")
                        .HasColumnType("float");

                    b.Property<double>("Conc_D3")
                        .HasColumnType("float");

                    b.Property<double>("Conc_D4")
                        .HasColumnType("float");

                    b.Property<double>("Conc_D5")
                        .HasColumnType("float");

                    b.Property<double>("Conc_Dust")
                        .HasColumnType("float");

                    b.Property<double>("Conc_H2S")
                        .HasColumnType("float");

                    b.Property<double>("Conc_NO")
                        .HasColumnType("float");

                    b.Property<double>("Conc_NO2")
                        .HasColumnType("float");

                    b.Property<double>("Conc_NOx")
                        .HasColumnType("float");

                    b.Property<double>("Conc_SO2")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Emis_CH4")
                        .HasColumnType("float");

                    b.Property<double>("Emis_CO")
                        .HasColumnType("float");

                    b.Property<double>("Emis_CO2")
                        .HasColumnType("float");

                    b.Property<double>("Emis_D1")
                        .HasColumnType("float");

                    b.Property<double>("Emis_D2")
                        .HasColumnType("float");

                    b.Property<double>("Emis_D3")
                        .HasColumnType("float");

                    b.Property<double>("Emis_D4")
                        .HasColumnType("float");

                    b.Property<double>("Emis_D5")
                        .HasColumnType("float");

                    b.Property<double>("Emis_Dust")
                        .HasColumnType("float");

                    b.Property<double>("Emis_H2S")
                        .HasColumnType("float");

                    b.Property<double>("Emis_NO")
                        .HasColumnType("float");

                    b.Property<double>("Emis_NO2")
                        .HasColumnType("float");

                    b.Property<double>("Emis_NOx")
                        .HasColumnType("float");

                    b.Property<double>("Emis_SO2")
                        .HasColumnType("float");

                    b.Property<double>("Flow")
                        .HasColumnType("float");

                    b.Property<double>("H2O")
                        .HasColumnType("float");

                    b.Property<int>("Mode_ASK")
                        .HasColumnType("int");

                    b.Property<double>("O2_Dry")
                        .HasColumnType("float");

                    b.Property<double>("O2_Wet")
                        .HasColumnType("float");

                    b.Property<int>("PDZ_Fuel")
                        .HasColumnType("int");

                    b.Property<double>("Pressure")
                        .HasColumnType("float");

                    b.Property<double>("Speed")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.Property<double>("Temperature_KIP")
                        .HasColumnType("float");

                    b.Property<double>("Temperature_NOx")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("AVG_20_MINUTE");
                });

            modelBuilder.Entity("ASK.Models.PDZ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Add_Conc_1")
                        .HasColumnType("float");

                    b.Property<double>("Add_Conc_2")
                        .HasColumnType("float");

                    b.Property<double>("Add_Conc_3")
                        .HasColumnType("float");

                    b.Property<double>("Add_Conc_4")
                        .HasColumnType("float");

                    b.Property<double>("Add_Conc_5")
                        .HasColumnType("float");

                    b.Property<double>("Add_Emis_1")
                        .HasColumnType("float");

                    b.Property<double>("Add_Emis_2")
                        .HasColumnType("float");

                    b.Property<double>("Add_Emis_3")
                        .HasColumnType("float");

                    b.Property<double>("Add_Emis_4")
                        .HasColumnType("float");

                    b.Property<double>("Add_Emis_5")
                        .HasColumnType("float");

                    b.Property<double>("CH4_Conc")
                        .HasColumnType("float");

                    b.Property<double>("CH4_Emis")
                        .HasColumnType("float");

                    b.Property<double>("CO2_Conc")
                        .HasColumnType("float");

                    b.Property<double>("CO2_Emis")
                        .HasColumnType("float");

                    b.Property<double>("CO_Conc")
                        .HasColumnType("float");

                    b.Property<double>("CO_Emis")
                        .HasColumnType("float");

                    b.Property<bool>("Current")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Dust_Conc")
                        .HasColumnType("float");

                    b.Property<double>("Dust_Emis")
                        .HasColumnType("float");

                    b.Property<double>("H2S_Conc")
                        .HasColumnType("float");

                    b.Property<double>("H2S_Emis")
                        .HasColumnType("float");

                    b.Property<double>("NO2_Conc")
                        .HasColumnType("float");

                    b.Property<double>("NO2_Emis")
                        .HasColumnType("float");

                    b.Property<double>("NO_Conc")
                        .HasColumnType("float");

                    b.Property<double>("NO_Emis")
                        .HasColumnType("float");

                    b.Property<double>("NOx_Conc")
                        .HasColumnType("float");

                    b.Property<double>("NOx_Emis")
                        .HasColumnType("float");

                    b.Property<int>("NumberPDZ")
                        .HasColumnType("int");

                    b.Property<double>("SO2_Conc")
                        .HasColumnType("float");

                    b.Property<double>("SO2_Emis")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("PDZ");
                });
#pragma warning restore 612, 618
        }
    }
}
