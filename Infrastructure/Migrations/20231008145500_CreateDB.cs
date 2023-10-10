using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "Users",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "Roles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "Roles",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "Roles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "RoleClaims",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "RoleClaims",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "RoleClaims",
                newName: "CreatedAt");

            migrationBuilder.CreateTable(
                name: "case",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reason = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    murderweapon = table.Column<string>(name: "murder_weapon", type: "nvarchar(100)", nullable: true),
                    startdate = table.Column<DateTime>(name: "start_date", type: "datetime", nullable: false),
                    enddate = table.Column<DateTime>(name: "end_date", type: "datetime", nullable: true),
                    typeofviolation = table.Column<short>(name: "type_of_violation", type: "smallint", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    charge = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "crime_reporting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportername = table.Column<string>(name: "reporter_name", type: "nvarchar(100)", nullable: false),
                    reporteremail = table.Column<string>(name: "reporter_email", type: "varchar(100)", nullable: true),
                    reporterphone = table.Column<string>(name: "reporter_phone", type: "nvarchar(15)", nullable: false),
                    reporteraddress = table.Column<string>(name: "reporter_address", type: "nvarchar(200)", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crime_reporting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "criminal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    anothername = table.Column<string>(name: "another_name", type: "nvarchar(100)", nullable: false),
                    CCCDCMND = table.Column<string>(name: "CCCD/CMND", type: "varchar(12)", nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "varchar(15)", nullable: false),
                    phonemodel = table.Column<string>(name: "phone_model", type: "nvarchar(100)", nullable: false),
                    careerandworkplace = table.Column<string>(name: "career_and_workplace", type: "nvarchar(300)", nullable: false),
                    characteristics = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    hometown = table.Column<string>(name: "home_town", type: "nvarchar(200)", nullable: false),
                    ethnicity = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    religion = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    fathername = table.Column<string>(name: "father_name", type: "nvarchar(100)", nullable: false),
                    fatherCCCDCMND = table.Column<string>(name: "father_CCCD/CMND", type: "varchar(12)", nullable: false),
                    fatherbirthday = table.Column<DateTime>(name: "father_birthday", type: "datetime", nullable: false),
                    mothername = table.Column<string>(name: "mother_name", type: "nvarchar(100)", nullable: false),
                    motherCCCDCMND = table.Column<string>(name: "mother_CCCD/CMND", type: "varchar(12)", nullable: false),
                    motherbirthday = table.Column<DateTime>(name: "mother_birthday", type: "datetime", nullable: false),
                    permanentresidence = table.Column<string>(name: "permanent_residence", type: "nvarchar(200)", nullable: false),
                    currentaccommodation = table.Column<string>(name: "current_accommodation", type: "nvarchar(200)", nullable: false),
                    entryandexitinformation = table.Column<string>(name: "entry_and_exit_information", type: "nvarchar(500)", nullable: true),
                    facebook = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    zalo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    othersocialnetworks = table.Column<string>(name: "other_social_networks", type: "nvarchar(300)", nullable: true),
                    gameaccount = table.Column<string>(name: "game_account", type: "nvarchar(100)", nullable: true),
                    research = table.Column<string>(type: "text", nullable: true),
                    vehicles = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    dangerouslevel = table.Column<string>(name: "dangerous_level", type: "nvarchar(200)", nullable: true),
                    approacharrange = table.Column<string>(name: "approach_arrange", type: "text", nullable: true),
                    dateofmostrecentcrime = table.Column<DateTime>(name: "date_of_most_recent_crime", type: "datetime", nullable: false),
                    releasedate = table.Column<DateTime>(name: "release_date", type: "datetime", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    otherinformation = table.Column<string>(name: "other_information", type: "nvarchar(500)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criminal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CMNDCCCD = table.Column<string>(name: "CMND/CCCD", type: "varchar(12)", nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    phonenumber = table.Column<string>(name: "phone_number", type: "varchar(15)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    image = table.Column<string>(type: "varchar(500)", nullable: true),
                    isactive = table.Column<bool>(name: "is_active", type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "nvarchar(50)", nullable: false),
                    filepath = table.Column<string>(name: "file_path", type: "nvarchar(500)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_image_case_case_id",
                        column: x => x.caseid,
                        principalTable: "case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evidence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evidence_case_case_id",
                        column: x => x.caseid,
                        principalTable: "case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "witness",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CMNDCCCD = table.Column<string>(name: "CMND/CCCD", type: "varchar(12)", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "varchar(15)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    testimony = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_witness", x => x.Id);
                    table.ForeignKey(
                        name: "FK_witness_case_case_id",
                        column: x => x.caseid,
                        principalTable: "case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportingImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportingid = table.Column<long>(name: "reporting_id", type: "bigint", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "nvarchar(50)", nullable: false),
                    filepath = table.Column<string>(name: "file_path", type: "nvarchar(500)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportingImages_crime_reporting_reporting_id",
                        column: x => x.reportingid,
                        principalTable: "crime_reporting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_criminal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    criminalid = table.Column<long>(name: "criminal_id", type: "bigint", nullable: false),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_criminal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_criminal_case_case_id",
                        column: x => x.caseid,
                        principalTable: "case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_case_criminal_criminal_criminal_id",
                        column: x => x.criminalid,
                        principalTable: "criminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "criminal_image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    criminalid = table.Column<long>(name: "criminal_id", type: "bigint", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "nvarchar(50)", nullable: false),
                    filepath = table.Column<string>(name: "file_path", type: "nvarchar(500)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criminal_image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_criminal_image_criminal_criminal_id",
                        column: x => x.criminalid,
                        principalTable: "criminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wanted_criminal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    criminalid = table.Column<long>(name: "criminal_id", type: "bigint", nullable: false),
                    wantedtype = table.Column<short>(name: "wanted_type", type: "smallint", nullable: false),
                    currentactivity = table.Column<string>(name: "current_activity", type: "nvarchar(200)", nullable: true),
                    wanteddecisionno = table.Column<string>(name: "wanted_decision_no", type: "nvarchar(50)", nullable: false),
                    wanteddecisionday = table.Column<DateTime>(name: "wanted_decision_day", type: "date", nullable: false),
                    decisionmakingunit = table.Column<string>(name: "decision_making_unit", type: "nvarchar(100)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wanted_criminal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_wanted_criminal_criminal_criminal_id",
                        column: x => x.criminalid,
                        principalTable: "criminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_case_criminal_case_id",
                table: "case_criminal",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "IX_case_criminal_criminal_id",
                table: "case_criminal",
                column: "criminal_id");

            migrationBuilder.CreateIndex(
                name: "IX_case_image_case_id",
                table: "case_image",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "IX_criminal_image_criminal_id",
                table: "criminal_image",
                column: "criminal_id");

            migrationBuilder.CreateIndex(
                name: "IX_evidence_case_id",
                table: "evidence",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "IX_ReportingImages_reporting_id",
                table: "ReportingImages",
                column: "reporting_id");

            migrationBuilder.CreateIndex(
                name: "IX_wanted_criminal_criminal_id",
                table: "wanted_criminal",
                column: "criminal_id");

            migrationBuilder.CreateIndex(
                name: "IX_witness_case_id",
                table: "witness",
                column: "case_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case_criminal");

            migrationBuilder.DropTable(
                name: "case_image");

            migrationBuilder.DropTable(
                name: "criminal_image");

            migrationBuilder.DropTable(
                name: "evidence");

            migrationBuilder.DropTable(
                name: "ReportingImages");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "wanted_criminal");

            migrationBuilder.DropTable(
                name: "witness");

            migrationBuilder.DropTable(
                name: "crime_reporting");

            migrationBuilder.DropTable(
                name: "criminal");

            migrationBuilder.DropTable(
                name: "case");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "Identity",
                table: "Users",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "Identity",
                table: "Users",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "Identity",
                table: "Roles",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "Identity",
                table: "Roles",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "Roles",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "Identity",
                table: "RoleClaims",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "Identity",
                table: "RoleClaims",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "RoleClaims",
                newName: "CreatedOn");
        }
    }
}
