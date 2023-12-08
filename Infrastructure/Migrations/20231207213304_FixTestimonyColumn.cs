using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTestimonyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testimony",
                table: "witness");

            migrationBuilder.AddColumn<string>(
                name: "testimony",
                table: "case_witness",
                type: "text",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "testimony",
                table: "case_victim",
                type: "text",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "testimony",
                table: "case_criminal",
                type: "text",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "case",
                type: "text",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testimony",
                table: "case_witness");

            migrationBuilder.DropColumn(
                name: "testimony",
                table: "case_victim");

            migrationBuilder.DropColumn(
                name: "testimony",
                table: "case_criminal");

            migrationBuilder.DropColumn(
                name: "description",
                table: "case");

            migrationBuilder.AddColumn<string>(
                name: "testimony",
                table: "witness",
                type: "text",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
