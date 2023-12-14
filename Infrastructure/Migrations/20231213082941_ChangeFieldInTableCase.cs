using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldInTableCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "murder_weapon",
                table: "case");

            migrationBuilder.AddColumn<long>(
                name: "evidence_id",
                table: "case_image",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "charge",
                table: "case_criminal",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "case_criminal",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "type_of_violation",
                table: "case_criminal",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "weapon",
                table: "case_criminal",
                type: "nvarchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "evidence_id",
                table: "case_image");

            migrationBuilder.DropColumn(
                name: "charge",
                table: "case_criminal");

            migrationBuilder.DropColumn(
                name: "reason",
                table: "case_criminal");

            migrationBuilder.DropColumn(
                name: "type_of_violation",
                table: "case_criminal");

            migrationBuilder.DropColumn(
                name: "weapon",
                table: "case_criminal");

            migrationBuilder.AddColumn<string>(
                name: "murder_weapon",
                table: "case",
                type: "nvarchar(100)",
                nullable: true);
        }
    }
}
