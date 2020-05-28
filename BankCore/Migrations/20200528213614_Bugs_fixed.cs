using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Bugs_fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Excecution_Date",
                table: "Transfers");

           migrationBuilder.DropColumn(
                name: "Openinig_Date",
                table: "Bank_Accounts");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Currencies",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "Execution_Date",
                table: "Transfers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "Installments_Count",
                table: "Loans",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Installments_Count",
                table: "Loan_Applications",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Opening_Date",
                table: "Bank_Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ID_Accounts_Login",
                table: "Accounts",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Login",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Execution_Date",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Opening_Date",
                table: "Bank_Accounts");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Currencies",
                newName: "name");

            migrationBuilder.AddColumn<DateTime>(
                name: "Excecution_Date",
                table: "Transfers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Installments_Count",
                table: "Loans",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "Installments_Count",
                table: "Loan_Applications",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<DateTime>(
                name: "Openinig_Date",
                table: "Bank_Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Accounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
