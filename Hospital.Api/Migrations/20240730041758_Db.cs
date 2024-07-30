using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
           name: "Declarations",
           columns: table => new
           {
               Id = table.Column<long>(type: "bigint", nullable: false)
                   .Annotation("SqlServer:Identity", "1, 1"),
               Address = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
               Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
               Creator = table.Column<long>(type: "bigint", nullable: true),
               Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
               DeletedBy = table.Column<long>(type: "bigint", nullable: true),
               Did = table.Column<int>(type: "int", nullable: false),
               Dname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
               Dob = table.Column<DateTime>(type: "DATETIME", nullable: false),
               Gender = table.Column<int>(type: "int", nullable: false),
               Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
               Modifier = table.Column<long>(type: "bigint", nullable: true),
               Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
               Phone = table.Column<string>(type: "NVARCHAR(12)", nullable: false),
               Pid = table.Column<int>(type: "int", nullable: false),
               Pname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
               ServiceId = table.Column<long>(type: "bigint", nullable: false),
               Wid = table.Column<int>(type: "int", nullable: false),
               Wname = table.Column<string>(type: "NVARCHAR(255)", nullable: true)
           },
           constraints: table =>
           {
               table.PrimaryKey("PK_Declarations", x => x.Id);
           });

            migrationBuilder.CreateIndex(
                name: "IX_Declarations_ServiceId",
                table: "Declarations",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
