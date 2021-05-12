﻿// <auto-generated />
using System;
using BAI3.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BAI3.Migrations
{
    [DbContext(typeof(BaiContext))]
    [Migration("20201212113940_migration")]
    partial class migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BAI3.Models.FragmentalPasswordSchema", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("actualPasswordSchema")
                        .HasColumnType("int");

                    b.Property<int>("passwordSice")
                        .HasColumnType("int");

                    b.Property<string>("schema")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FragmentalPasswordSchemas");
                });

            modelBuilder.Entity("BAI3.Models.LoginAttepmts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("attempt")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LoginAttempts");
                });

            modelBuilder.Entity("BAI3.Models.LogowanieZdarzen", b =>
                {
                    b.Property<int>("logowanieZdarzenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("haslo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("iloscNieudanychLogowan")
                        .HasColumnType("int");

                    b.Property<DateTime>("ostatniaProbaLogowania")
                        .HasColumnType("datetime2");

                    b.HasKey("logowanieZdarzenId");

                    b.ToTable("LogowanieZdarzens");
                });

            modelBuilder.Entity("BAI3.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("blokada")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dataOstatniegoNieudanegoLogowania")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dataOstatniegoUdanegoLogowania")
                        .HasColumnType("datetime2");

                    b.Property<string>("haslo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("iloscNieUdanychProbLogowania")
                        .HasColumnType("int");

                    b.Property<string>("imie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nazwisko")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("wlaczenieBlokadyKonta")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
