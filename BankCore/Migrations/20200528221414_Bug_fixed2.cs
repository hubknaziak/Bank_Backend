using Microsoft.EntityFrameworkCore.Migrations;

namespace BankCore.Migrations
{
    public partial class Bug_fixed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Accounts",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Accounts",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 9);
        }
    }
}
