using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U52 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expertise",
                table: "mcs_doctors");

            migrationBuilder.AddColumn<string>(
                name: "ExpertiseEn",
                table: "mcs_doctors",
                type: "NVARCHAR(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpertiseVn",
                table: "mcs_doctors",
                type: "NVARCHAR(512)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpertiseEn",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "ExpertiseVn",
                table: "mcs_doctors");

            migrationBuilder.AddColumn<string>(
                name: "Expertise",
                table: "mcs_doctors",
                type: "NVARCHAR(255)",
                nullable: true);
        }
    }
}
