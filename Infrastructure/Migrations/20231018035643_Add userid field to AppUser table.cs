using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdduseridfieldtoAppUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "user");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                schema: "Identity",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AddColumn<ulong>(
                name: "is_active",
                table: "user",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);
        }
    }
}
