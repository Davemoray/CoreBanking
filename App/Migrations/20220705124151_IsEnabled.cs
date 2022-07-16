using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    public partial class IsEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    BranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortCode = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.BranchID);
                });

            migrationBuilder.CreateTable(
                name: "GlCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MainCategory = table.Column<int>(type: "int", nullable: false),
                    GlCategoryCategoryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlCategory", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_GlCategory_GlCategory_GlCategoryCategoryID",
                        column: x => x.GlCategoryCategoryID,
                        principalTable: "GlCategory",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "GlAccountModel",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    AccountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GlCategoryID = table.Column<int>(type: "int", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlAccountModel", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_GlAccountModel_Branch_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branch",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlAccountModel_GlCategory_GlCategoryID",
                        column: x => x.GlCategoryID,
                        principalTable: "GlCategory",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlAccountModel_BranchID",
                table: "GlAccountModel",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_GlAccountModel_GlCategoryID",
                table: "GlAccountModel",
                column: "GlCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_GlCategory_GlCategoryCategoryID",
                table: "GlCategory",
                column: "GlCategoryCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlAccountModel");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "GlCategory");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetUsers");
        }
    }
}
