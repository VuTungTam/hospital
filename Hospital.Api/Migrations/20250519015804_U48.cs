using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AliasLogin",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "Json",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "AliasLogin",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "Json",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "AliasLogin",
                table: "mcs_customers");

            migrationBuilder.DropColumn(
                name: "Json",
                table: "mcs_customers");

            migrationBuilder.DropColumn(
                name: "PhoneVerified",
                table: "mcs_customers");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "mcs_customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AliasLogin",
                table: "mcs_employees",
                type: "NVARCHAR(128)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "mcs_employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "mcs_employees",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "mcs_employees",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AliasLogin",
                table: "mcs_doctors",
                type: "NVARCHAR(128)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "mcs_doctors",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "mcs_doctors",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "mcs_doctors",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AliasLogin",
                table: "mcs_customers",
                type: "NVARCHAR(128)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "mcs_customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneVerified",
                table: "mcs_customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "mcs_customers",
                type: "NVARCHAR(255)",
                nullable: true);
        }
    }
}
