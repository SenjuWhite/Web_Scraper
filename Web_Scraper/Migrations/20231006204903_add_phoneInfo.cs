using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Scraper.Migrations
{
    /// <inheritdoc />
    public partial class add_phoneInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhoneInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Screen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Camera = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memory = table.Column<int>(type: "int", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RAM = table.Column<int>(type: "int", nullable: false),
                    Battery = table.Column<int>(type: "int", nullable: false),
                    Case = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneInfo");
        }
    }
}
