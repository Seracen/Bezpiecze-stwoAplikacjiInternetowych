using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BAI3.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attempt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogowanieZdarzens",
                columns: table => new
                {
                    logowanieZdarzenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: true),
                    haslo = table.Column<string>(nullable: true),
                    iloscNieudanychLogowan = table.Column<int>(nullable: false),
                    ostatniaProbaLogowania = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogowanieZdarzens", x => x.logowanieZdarzenId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(nullable: true),
                    haslo = table.Column<string>(nullable: true),
                    imie = table.Column<string>(nullable: true),
                    nazwisko = table.Column<string>(nullable: true),
                    iloscNieUdanychProbLogowania = table.Column<int>(nullable: false),
                    wlaczenieBlokadyKonta = table.Column<string>(nullable: true),
                    blokada = table.Column<string>(nullable: true),
                    dataOstatniegoNieudanegoLogowania = table.Column<DateTime>(nullable: false),
                    dataOstatniegoUdanegoLogowania = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginAttempts");

            migrationBuilder.DropTable(
                name: "LogowanieZdarzens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
