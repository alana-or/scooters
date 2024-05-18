using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Motto.Entities;

namespace Motos.Data.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Motos",
            columns: table => new
            {
                id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ano = table.Column<int>(nullable: false),
                modelo = table.Column<string>(maxLength: 100, nullable: false),
                placa = table.Column<string>(maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Motos", x => x.id);
            });


        migrationBuilder.CreateTable(
            name: "MotosLog2024",
            columns: table => new
            {
                message = table.Column<string>(nullable: false),
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Motos");

        migrationBuilder.DropTable(
            name: "MotosLog2024");
    }
}
