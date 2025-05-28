using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Symptoms",
                table: "tbl_specialties",
                newName: "SymptomVns");

            migrationBuilder.AddColumn<string>(
                name: "SymptomEns",
                table: "tbl_specialties",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SymptomEns",
                table: "tbl_specialties");

            migrationBuilder.RenameColumn(
                name: "SymptomVns",
                table: "tbl_specialties",
                newName: "Symptoms");
        }
    }
}
