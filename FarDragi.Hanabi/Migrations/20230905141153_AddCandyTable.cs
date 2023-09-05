using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarDragi.Hanabi.Migrations
{
    public partial class AddCandyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candies",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candies");
        }
    }
}
