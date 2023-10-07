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
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "Users",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "Roles",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "Roles",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "Roles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "RoleClaims",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "RoleClaims",
                newName: "UpdateBy");

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
                    reason = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    murderweapon = table.Column<string>(name: "murder_weapon", type: "nvarchar(100)", nullable: false),
                    startdate = table.Column<DateTime>(name: "start_date", type: "datetime", nullable: true),
                    enddate = table.Column<DateTime>(name: "end_date", type: "datetime", nullable: false),
                    typeofviolation = table.Column<short>(name: "type_of_violation", type: "smallint", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true),
                    charge = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_criminal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    criminalid = table.Column<long>(name: "criminal_id", type: "bigint", nullable: true),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_criminal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: true),
                    filename = table.Column<string>(name: "file_name", type: "nvarchar(50)", nullable: true),
                    filepath = table.Column<string>(name: "file_path", type: "nvarchar(500)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "crime_reporting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportername = table.Column<string>(name: "reporter_name", type: "nvarchar(100)", nullable: true),
                    reporteremail = table.Column<string>(name: "reporter_email", type: "varchar(100)", nullable: false),
                    reporterphone = table.Column<string>(name: "reporter_phone", type: "nvarchar(15)", nullable: true),
                    reporteraddress = table.Column<string>(name: "reporter_address", type: "nvarchar(200)", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true),
                    note = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    anothername = table.Column<string>(name: "another_name", type: "nvarchar(100)", nullable: true),
                    CCCDCMND = table.Column<string>(name: "CCCD/CMND", type: "varchar(12)", nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    phonenumber = table.Column<string>(name: "phone_number", type: "varchar(15)", nullable: true),
                    phonemodel = table.Column<string>(name: "phone_model", type: "nvarchar(100)", nullable: true),
                    careerandworkplace = table.Column<string>(name: "career_and_workplace", type: "nvarchar(300)", nullable: true),
                    characteristics = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    hometown = table.Column<string>(name: "home_town", type: "nvarchar(200)", nullable: true),
                    ethnicity = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    religion = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    fathername = table.Column<string>(name: "father_name", type: "nvarchar(100)", nullable: true),
                    fatherCCCDCMND = table.Column<string>(name: "father_CCCD/CMND", type: "varchar(12)", nullable: true),
                    fatherbirthday = table.Column<DateTime>(name: "father_birthday", type: "datetime", nullable: true),
                    mothername = table.Column<string>(name: "mother_name", type: "nvarchar(100)", nullable: true),
                    motherCCCDCMND = table.Column<string>(name: "mother_CCCD/CMND", type: "varchar(12)", nullable: true),
                    motherbirthday = table.Column<DateTime>(name: "mother_birthday", type: "datetime", nullable: true),
                    permanentresidence = table.Column<string>(name: "permanent_residence", type: "nvarchar(200)", nullable: true),
                    currentaccommodation = table.Column<string>(name: "current_accommodation", type: "nvarchar(200)", nullable: true),
                    entryandexitinformation = table.Column<string>(name: "entry_and_exit_information", type: "nvarchar(500)", nullable: false),
                    facebook = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    zalo = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    othersocialnetworks = table.Column<string>(name: "other_social_networks", type: "nvarchar(300)", nullable: false),
                    gameaccount = table.Column<string>(name: "game_account", type: "nvarchar(100)", nullable: false),
                    research = table.Column<string>(type: "text", nullable: false),
                    vehicles = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    dangerouslevel = table.Column<string>(name: "dangerous_level", type: "nvarchar(200)", nullable: false),
                    approacharrange = table.Column<string>(name: "approach_arrange", type: "text", nullable: false),
                    dateofmostrecentcrime = table.Column<DateTime>(name: "date_of_most_recent_crime", type: "datetime", nullable: true),
                    releasedate = table.Column<DateTime>(name: "release_date", type: "datetime", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true),
                    ortherinformation = table.Column<string>(name: "orther_information", type: "nvarchar(500)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criminal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "criminal_image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    criminalid = table.Column<long>(name: "criminal_id", type: "bigint", nullable: true),
                    filename = table.Column<string>(name: "file_name", type: "nvarchar(50)", nullable: true),
                    filepath = table.Column<string>(name: "file_path", type: "nvarchar(500)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criminal_image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "evidence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evidence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportingImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportingid = table.Column<long>(name: "reporting_id", type: "bigint", nullable: true),
                    filename = table.Column<string>(name: "file_name", type: "nvarchar(50)", nullable: true),
                    filepath = table.Column<string>(name: "file_path", type: "nvarchar(500)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CMNDCCCD = table.Column<string>(name: "CMND/CCCD", type: "varchar(12)", nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    phonenumber = table.Column<string>(name: "phone_number", type: "varchar(15)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    email = table.Column<string>(type: "varchar(100)", nullable: true),
                    image = table.Column<string>(type: "varchar(500)", nullable: false),
                    isactive = table.Column<bool>(name: "is_active", type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wanted_criminal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    criminalid = table.Column<long>(name: "criminal_id", type: "bigint", nullable: true),
                    wantedtype = table.Column<short>(name: "wanted_type", type: "smallint", nullable: true),
                    currentactivity = table.Column<string>(name: "current_activity", type: "nvarchar(200)", nullable: false),
                    wanteddecisionno = table.Column<string>(name: "wanted_decision_no", type: "nvarchar(50)", nullable: true),
                    wanteddecisionday = table.Column<DateTime>(name: "wanted_decision_day", type: "date", nullable: true),
                    decisionmakingunit = table.Column<string>(name: "decision_making_unit", type: "nvarchar(100)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wanted_criminal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "witness",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CMNDCCCD = table.Column<string>(name: "CMND/CCCD", type: "varchar(12)", nullable: true),
                    phonenumber = table.Column<string>(name: "phone_number", type: "varchar(15)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    testimony = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    caseid = table.Column<long>(name: "case_id", type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_witness", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case");

            migrationBuilder.DropTable(
                name: "case_criminal");

            migrationBuilder.DropTable(
                name: "case_image");

            migrationBuilder.DropTable(
                name: "crime_reporting");

            migrationBuilder.DropTable(
                name: "criminal");

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

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                schema: "Identity",
                table: "Users",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                schema: "Identity",
                table: "Users",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                schema: "Identity",
                table: "Roles",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                schema: "Identity",
                table: "Roles",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "Roles",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                schema: "Identity",
                table: "RoleClaims",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
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
