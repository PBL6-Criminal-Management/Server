using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseIdToWantedCriminalTableAndAvatarFieldToCriminalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "case_id",
                table: "wanted_criminal",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "criminal",
                type: "varchar(500)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_wanted_criminal_case_id",
                table: "wanted_criminal",
                column: "case_id");

            migrationBuilder.AddForeignKey(
                name: "FK_wanted_criminal_case_case_id",
                table: "wanted_criminal",
                column: "case_id",
                principalTable: "case",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wanted_criminal_case_case_id",
                table: "wanted_criminal");

            migrationBuilder.DropIndex(
                name: "IX_wanted_criminal_case_id",
                table: "wanted_criminal");

            migrationBuilder.DropColumn(
                name: "case_id",
                table: "wanted_criminal");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "criminal");
        }
    }
}
