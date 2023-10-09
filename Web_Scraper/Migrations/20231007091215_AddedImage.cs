using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Scraper.Migrations
{
    /// <inheritdoc />
    public partial class AddedImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "PhoneInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "PhoneInfo");
        }
    }
}
