using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    public partial class Branchdroppedcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortCode",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Branch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SortCode",
                table: "Branch",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Branch",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
