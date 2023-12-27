using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderAndBirthdayToWitnessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "birthday",
                table: "witness",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "gender",
                table: "witness",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthday",
                table: "witness");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "witness");
        }
    }
}
