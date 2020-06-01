using System;
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
                    Id_account = table.Column<int>(nullable: false),
                    Login = table.Column<string>(unicode: false, maxLength: 9, nullable: false),
                    Password = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    First_name = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Last_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account", x => x.Id_account);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id_currency = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Exchange_rate = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Currency", x => x.Id_currency);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id_administrator = table.Column<int>(nullable: false),
                    Employment_date = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Administ", x => x.Id_administrator);
                    table.ForeignKey(
                        name: "FK__Administr__id_ad",
                        column: x => x.Id_administrator,
                        principalTable: "Accounts",
                        principalColumn: "Id_account",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id_client = table.Column<int>(nullable: false),
                    Phone_number = table.Column<string>(unicode: false, maxLength: 12, nullable: false),
                    Address = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Client", x => x.Id_client);
                    table.ForeignKey(
                        name: "FK__Client__id_clien",
                        column: x => x.Id_client,
                        principalTable: "Accounts",
                        principalColumn: "Id_account",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bank_accounts",
                columns: table => new
                {
                    Id_bank_account = table.Column<int>(nullable: false),
                    Opening_date = table.Column<DateTime>(type: "date", nullable: false),
                    Account_balance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    Client = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bank_acc", x => x.Id_bank_account);
                    table.ForeignKey(
                        name: "FK__Bank_acco__clien",
                        column: x => x.Client,
                        principalTable: "Clients",
                        principalColumn: "Id_client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Bank_acco__curre",
                        column: x => x.Currency,
                        principalTable: "Currencies",
                        principalColumn: "Id_currency",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "loan_applications",
                columns: table => new
                {
                    Id_loan_application = table.Column<int>(nullable: false),
                    Submission_date = table.Column<DateTime>(type: "date", nullable: false),
                    Decision_date = table.Column<DateTime>(type: "date", nullable: false),
                    Installments_count = table.Column<decimal>(type: "numeric(3, 0)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Repayment_time = table.Column<decimal>(type: "numeric(3, 0)", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Client = table.Column<int>(nullable: false),
                    administrator = table.Column<int>(nullable: false),
                    Bank_account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Loan_app", x => x.Id_loan_application);
                    table.ForeignKey(
                        name: "FK__Loan_appl__admin",
                        column: x => x.administrator,
                        principalTable: "Administrators",
                        principalColumn: "Id_administrator",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Loan_appl__bank",
                        column: x => x.Bank_account,
                        principalTable: "bank_accounts",
                        principalColumn: "Id_bank_account",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Loan_appl__clien__32767D0B",
                        column: x => x.Client,
                        principalTable: "Clients",
                        principalColumn: "Id_client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id_loan = table.Column<int>(nullable: false),
                    Total_amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Outstanding_amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Rate_of_interest = table.Column<decimal>(type: "decimal(4, 2)", nullable: false),
                    Installments_count = table.Column<decimal>(type: "numeric(3, 0)", nullable: false),
                    Installment = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Granting_date = table.Column<DateTime>(type: "date", nullable: false),
                    End_of_repayment = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Client = table.Column<int>(nullable: false),
                    Administrator = table.Column<int>(nullable: false),
                    Bank_account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Loan", x => x.Id_loan);
                    table.ForeignKey(
                        name: "FK__Loan__administra",
                        column: x => x.Administrator,
                        principalTable: "Administrators",
                        principalColumn: "Id_administrator",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Loan__bank_accou",
                        column: x => x.Bank_account,
                        principalTable: "bank_accounts",
                        principalColumn: "Id_bank_account",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Loan__client",
                        column: x => x.Client,
                        principalTable: "Clients",
                        principalColumn: "Id_client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id_transfer = table.Column<int>(nullable: false),
                    Sending_date = table.Column<DateTime>(type: "date", nullable: false),
                    Execution_date = table.Column<DateTime>(type: "date", nullable: false),
                    Title = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Receiver = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Sender_bank_account = table.Column<int>(nullable: false),
                    Receiver_bank_account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transfer", x => x.Id_transfer);
                    table.ForeignKey(
                        name: "FK__Transfer__receiv",
                        column: x => x.Receiver_bank_account,
                        principalTable: "bank_accounts",
                        principalColumn: "Id_bank_account",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transfer__sender",
                        column: x => x.Sender_bank_account,
                        principalTable: "bank_accounts",
                        principalColumn: "Id_bank_account",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Account",
                table: "Accounts",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_Client",
                table: "bank_accounts",
                column: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_Currency",
                table: "bank_accounts",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_administrator",
                table: "loan_applications",
                column: "administrator");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_Bank_account",
                table: "loan_applications",
                column: "Bank_account");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_Client",
                table: "loan_applications",
                column: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_Administrator",
                table: "Loans",
                column: "Administrator");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_Bank_account",
                table: "Loans",
                column: "Bank_account");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_Client",
                table: "Loans",
                column: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Receiver_bank_account",
                table: "Transfers",
                column: "Receiver_bank_account");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Sender_bank_account",
                table: "Transfers",
                column: "Sender_bank_account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loan_applications");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "bank_accounts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
