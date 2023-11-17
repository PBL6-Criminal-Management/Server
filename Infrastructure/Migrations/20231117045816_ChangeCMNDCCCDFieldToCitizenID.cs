using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCMNDCCCDFieldToCitizenID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CMND/CCCD",
                table: "witness",
                newName: "citizen_id");

            migrationBuilder.RenameColumn(
                name: "CMND/CCCD",
                table: "victim",
                newName: "citizen_id");

            migrationBuilder.RenameColumn(
                name: "CMND/CCCD",
                table: "user",
                newName: "citizen_id");

            migrationBuilder.RenameColumn(
                name: "mother_CMND/CCCD",
                table: "criminal",
                newName: "mother_citizen_id");

            migrationBuilder.RenameColumn(
                name: "father_CMND/CCCD",
                table: "criminal",
                newName: "father_citizen_id");

            migrationBuilder.RenameColumn(
                name: "CMND/CCCD",
                table: "criminal",
                newName: "citizen_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "citizen_id",
                table: "witness",
                newName: "CMND/CCCD");

            migrationBuilder.RenameColumn(
                name: "citizen_id",
                table: "victim",
                newName: "CMND/CCCD");

            migrationBuilder.RenameColumn(
                name: "citizen_id",
                table: "user",
                newName: "CMND/CCCD");

            migrationBuilder.RenameColumn(
                name: "mother_citizen_id",
                table: "criminal",
                newName: "mother_CMND/CCCD");

            migrationBuilder.RenameColumn(
                name: "father_citizen_id",
                table: "criminal",
                newName: "father_CMND/CCCD");

            migrationBuilder.RenameColumn(
                name: "citizen_id",
                table: "criminal",
                newName: "CMND/CCCD");
        }
    }
}
