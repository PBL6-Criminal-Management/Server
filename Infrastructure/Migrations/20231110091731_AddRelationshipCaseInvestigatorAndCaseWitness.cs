using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipCaseInvestigatorAndCaseWitness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_witness_case_case_id",
                table: "witness");

            migrationBuilder.DropIndex(
                name: "IX_witness_case_id",
                table: "witness");

            migrationBuilder.CreateTable(
                name: "case_investigator",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    case_id = table.Column<long>(type: "bigint", nullable: false),
                    investigator_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_deleted = table.Column<ulong>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_investigator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_investigator_case_case_id",
                        column: x => x.case_id,
                        principalTable: "case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_case_investigator_user_investigator_id",
                        column: x => x.investigator_id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "case_witness",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    case_id = table.Column<long>(type: "bigint", nullable: false),
                    witness_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_deleted = table.Column<ulong>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_witness", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_witness_case_case_id",
                        column: x => x.case_id,
                        principalTable: "case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_case_witness_witness_witness_id",
                        column: x => x.witness_id,
                        principalTable: "witness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_case_investigator_case_id",
                table: "case_investigator",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "IX_case_investigator_investigator_id",
                table: "case_investigator",
                column: "investigator_id");

            migrationBuilder.CreateIndex(
                name: "IX_case_witness_case_id",
                table: "case_witness",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "IX_case_witness_witness_id",
                table: "case_witness",
                column: "witness_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case_investigator");

            migrationBuilder.DropTable(
                name: "case_witness");

            migrationBuilder.CreateIndex(
                name: "IX_witness_case_id",
                table: "witness",
                column: "case_id");

            migrationBuilder.AddForeignKey(
                name: "FK_witness_case_case_id",
                table: "witness",
                column: "case_id",
                principalTable: "case",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
