using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id_account = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(maxLength: 9, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    First_name = table.Column<string>(nullable: false),
                    Last_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id_account);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id_Administrator = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Employment_Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id_Administrator);
                });

            migrationBuilder.CreateTable(
                name: "Bank_Accounts",
                columns: table => new
                {
                    Id_Bank_Account = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Opening_Date = table.Column<DateTime>(nullable: false),
                    Account_Balance = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    Client = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_Accounts", x => x.Id_Bank_Account);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id_Client = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Phone_Number = table.Column<string>(maxLength: 12, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id_Client);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id_Currency = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Exchange_Rate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id_Currency);
                });

            migrationBuilder.CreateTable(
                name: "Loan_Applications",
                columns: table => new
                {
                    Id_Loan_Application = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Submission_Date = table.Column<DateTime>(nullable: false),
                    Decicion_Date = table.Column<DateTime>(nullable: false),
                    Installments_Count = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Repayment_Time = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    Client = table.Column<int>(nullable: false),
                    Administrator = table.Column<int>(nullable: false),
                    Bank_Account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan_Applications", x => x.Id_Loan_Application);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id_Loan = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Total_Amount = table.Column<decimal>(nullable: false),
                    Outstanding_Amount = table.Column<decimal>(nullable: false),
                    Rate_Of_Interest = table.Column<decimal>(nullable: false),
                    Installments_Count = table.Column<decimal>(nullable: false),
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
                    table.PrimaryKey("PK_Loans", x => x.Id_Loan);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id_Transfer = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sending_Date = table.Column<DateTime>(nullable: false),
                    Execution_Date = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 30, nullable: false),
                    Receiver = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    Sender_Bank_Account = table.Column<int>(nullable: false),
                    Receiver_Bank_Account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id_Transfer);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Login",
                table: "Accounts",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "Bank_Accounts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Loan_Applications");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
