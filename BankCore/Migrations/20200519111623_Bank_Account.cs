using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Bank_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank_Accounts",
                columns: table => new
                {
                    Id_Bank_Account = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Openinig_Date = table.Column<DateTime>(nullable: false),
                    Account_Balance = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    Client = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_Accounts", x => x.Id_Bank_Account);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank_Accounts");
        }
    }
}
