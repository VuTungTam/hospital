using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "tbl_health_facilities",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "SummaryEn",
                table: "tbl_health_facilities",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SummaryVn",
                table: "tbl_health_facilities",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummaryEn",
                table: "tbl_health_facilities");

            migrationBuilder.DropColumn(
                name: "SummaryVn",
                table: "tbl_health_facilities");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "tbl_health_facilities",
                newName: "ImageUrl");
        }
    }
}
