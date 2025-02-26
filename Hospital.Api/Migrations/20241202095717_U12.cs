using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "HealthFacilities",
                type: "NVARCHAR(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "HealthFacilities");
        }
    }
}
