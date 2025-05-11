using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "DescriptionVn",
                table: "tbl_health_services");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "tbl_health_services",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionVn",
                table: "tbl_health_services",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");
        }
    }
}
