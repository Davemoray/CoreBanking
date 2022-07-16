using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    public partial class TillUserusername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TillUser_AspNetUsers_UserId",
                table: "TillUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TillUser",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TillUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TillUser_AspNetUsers_UserId",
                table: "TillUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TillUser_AspNetUsers_UserId",
                table: "TillUser");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TillUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TillUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TillUser_AspNetUsers_UserId",
                table: "TillUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
