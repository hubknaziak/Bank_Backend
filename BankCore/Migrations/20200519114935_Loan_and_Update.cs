using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Loan_and_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Loan_Applications",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Id_Loan",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "End_Of_Repayment",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Granting_Date",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Installment",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Outstanding_Amount",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Rate_Of_Interest",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Total_Amount",
                table: "Loan_Applications");

            migrationBuilder.AddColumn<int>(
                name: "Id_Loan_Application",
                table: "Loan_Applications",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Loan_Applications",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Decicion_Date",
                table: "Loan_Applications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Repayment_Time",
                table: "Loan_Applications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Submission_Date",
                table: "Loan_Applications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loan_Applications",
                table: "Loan_Applications",
                column: "Id_Loan_Application");

            migrationBuilder.CreateTable(
                name: "Loans",
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
                    table.PrimaryKey("PK_Loans", x => x.Id_Loan);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loan_Applications",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Id_Loan_Application",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Decicion_Date",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Repayment_Time",
                table: "Loan_Applications");

            migrationBuilder.DropColumn(
                name: "Submission_Date",
                table: "Loan_Applications");

            migrationBuilder.AddColumn<int>(
                name: "Id_Loan",
                table: "Loan_Applications",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "End_Of_Repayment",
                table: "Loan_Applications",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Granting_Date",
                table: "Loan_Applications",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Installment",
                table: "Loan_Applications",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Outstanding_Amount",
                table: "Loan_Applications",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate_Of_Interest",
                table: "Loan_Applications",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total_Amount",
                table: "Loan_Applications",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loan_Applications",
                table: "Loan_Applications",
                column: "Id_Loan");
        }
    }
}
