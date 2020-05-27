using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Loan_Application : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loan_Applications",
                columns: table => new
                {
                    Id_Loan = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Total_Amount = table.Column<decimal>(nullable: false),
                    Outstanding_Amount = table.Column<decimal>(nullable: false),
                    Rate_Of_Interest = table.Column<decimal>(nullable: false),
                    Installments_Count = table.Column<int>(nullable: false),
                    Installment = table.Column<decimal>(nullable: false),
                    Granting_Date = table.Column<DateTime>(nullable: false),
                    End_Of_Repayment = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    Client = table.Column<int>(nullable: false),
                    Administrator = table.Column<int>(nullable: false),
                    Bank_Account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan_Applications", x => x.Id_Loan);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loan_Applications");
        }
    }
}
