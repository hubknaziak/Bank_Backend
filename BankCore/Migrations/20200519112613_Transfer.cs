using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Transfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id_Transfer = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sending_Date = table.Column<DateTime>(nullable: false),
                    Excecution_Date = table.Column<DateTime>(nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
