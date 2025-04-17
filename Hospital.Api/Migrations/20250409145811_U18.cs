using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescriptionVn",
                table: "tbl_health_services",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "tbl_health_services",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionVn",
                table: "tbl_health_facilities",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "tbl_health_facilities",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionVn",
                table: "tbl_facility_types",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "tbl_facility_types",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "mcs_doctors",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescriptionVn",
                table: "tbl_health_services",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "tbl_health_services",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionVn",
                table: "tbl_health_facilities",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "tbl_health_facilities",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionVn",
                table: "tbl_facility_types",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "tbl_facility_types",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "mcs_doctors",
                type: "NVARCHAR(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);
        }
    }
}
