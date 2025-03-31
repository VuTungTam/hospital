using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "mcs_doctors");

            migrationBuilder.RenameColumn(
                name: "DoctorStatus",
                table: "mcs_doctors",
                newName: "DoctorTitle");

            migrationBuilder.AddColumn<int>(
                name: "DoctorDegree",
                table: "mcs_doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorRank",
                table: "mcs_doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorDegree",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "DoctorRank",
                table: "mcs_doctors");

            migrationBuilder.RenameColumn(
                name: "DoctorTitle",
                table: "mcs_doctors",
                newName: "DoctorStatus");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "mcs_doctors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
