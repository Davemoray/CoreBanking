using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    public partial class accountconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavingsInterestRate = table.Column<float>(type: "real", nullable: true),
                    LoanInterestRate = table.Column<float>(type: "real", nullable: true),
                    SavingsMinBalance = table.Column<float>(type: "real", nullable: true),
                    CurrentMinBalance = table.Column<float>(type: "real", nullable: true),
                    SavingsMaxDailyWithdrawal = table.Column<float>(type: "real", nullable: true),
                    CurrentMaxDailyWithdrawal = table.Column<float>(type: "real", nullable: true),
                    FinancialDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountConfiguration", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountConfiguration");
        }
    }
}
