using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarDragi.Hanabi.Migrations
{
    public partial class AddTreatingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treating",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treating", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Treating");
        }
    }
}
