using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class Db3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
            name: "FK_Declarations_Services_ServiceId",
            table: "Declarations",
            column: "ServiceId",
            principalTable: "HealthServices",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
            name: "FK_DeclarationSymptom_Declarations_DeclarationId",
            table: "DeclarationSymptom",
            column: "DeclarationId",
            principalTable: "Declarations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
