using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eid",
                table: "tbl_health_profiles");

            migrationBuilder.RenameColumn(
                name: "Ethinic",
                table: "tbl_health_profiles",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "tbl_health_profiles",
                newName: "Ethinic");

            migrationBuilder.AddColumn<int>(
                name: "Eid",
                table: "tbl_health_profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
