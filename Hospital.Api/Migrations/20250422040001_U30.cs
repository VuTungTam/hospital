using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationEn",
                table: "tbl_zones",
                type: "NVARCHAR(512)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationVn",
                table: "tbl_zones",
                type: "NVARCHAR(512)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationEn",
                table: "tbl_zones");

            migrationBuilder.DropColumn(
                name: "LocationVn",
                table: "tbl_zones");
        }
    }
}
