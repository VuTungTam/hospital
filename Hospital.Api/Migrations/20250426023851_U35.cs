using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tbl_facility_types");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_facility_types");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_facility_types");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tbl_facility_types");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tbl_facility_types");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "tbl_service_types",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionVn",
                table: "tbl_service_types",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "tbl_service_types",
                type: "NVARCHAR(512)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "tbl_service_types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "tbl_service_types",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "DescriptionVn",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "tbl_service_types");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "tbl_service_types");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_service_types",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "tbl_service_types",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_service_types",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "tbl_service_types",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tbl_service_types",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "tbl_service_types",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "tbl_service_types",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_facility_types",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "tbl_facility_types",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_facility_types",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "tbl_facility_types",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tbl_facility_types",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
