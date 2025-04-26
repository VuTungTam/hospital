using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "tbl_health_profiles",
                type: "NVARCHAR(12)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(12)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "tbl_health_profiles",
                type: "NVARCHAR(12)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(12)",
                oldNullable: true);
        }
    }
}
