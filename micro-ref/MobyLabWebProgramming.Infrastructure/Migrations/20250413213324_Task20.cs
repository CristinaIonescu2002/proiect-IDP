using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Task20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Reference");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Reference",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Reference");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Reference",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);
        }
    }
}
