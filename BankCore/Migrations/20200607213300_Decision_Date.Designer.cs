﻿// <auto-generated />
using System;
using BankCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankCore.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200607213300_Decision_Date")]
    partial class Decision_Date
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BankCore.Models.Account", b =>
                {
                    b.Property<int>("Id_account")
                        .HasColumnName("Id_account")
                        .HasColumnType("int");

                    b.Property<string>("First_name")
                        .IsRequired()
                        .HasColumnName("First_name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<string>("Last_name")
                        .IsRequired()
                        .HasColumnName("Last_name")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("Login")
                        .HasColumnType("varchar(9) CHARACTER SET utf8mb4")
                        .HasMaxLength(9)
                        .IsUnicode(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password")
                        .HasColumnType("varchar(250) CHARACTER SET utf8mb4")
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.HasKey("Id_account")
                        .HasName("PK__Account");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasName("UQ__Account");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankCore.Models.Administrator", b =>
                {
                    b.Property<int>("Id_Administrator")
                        .HasColumnName("Id_administrator")
                        .HasColumnType("int");

                    b.Property<DateTime>("Employment_Date")
                        .HasColumnName("Employment_date")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id_Administrator")
                        .HasName("PK__Administ");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("BankCore.Models.Bank_Account", b =>
                {
                    b.Property<int>("Id_Bank_Account")
                        .HasColumnName("Id_bank_account")
                        .HasColumnType("int");

                    b.Property<decimal>("Account_Balance")
                        .HasColumnName("Account_balance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("Client")
                        .HasColumnName("Client")
                        .HasColumnType("int");

                    b.Property<int>("Currency")
                        .HasColumnName("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime>("Opening_Date")
                        .HasColumnName("Opening_date")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id_Bank_Account")
                        .HasName("PK__Bank_acc");

                    b.HasIndex("Client");

                    b.HasIndex("Currency");

                    b.ToTable("bank_accounts");
                });

            modelBuilder.Entity("BankCore.Models.Client", b =>
                {
                    b.Property<int>("Id_Client")
                        .HasColumnName("Id_client")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnName("Address")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Phone_Number")
                        .IsRequired()
                        .HasColumnName("Phone_number")
                        .HasColumnType("varchar(12) CHARACTER SET utf8mb4")
                        .HasMaxLength(12)
                        .IsUnicode(false);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id_Client")
                        .HasName("PK__Client");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("BankCore.Models.Currency", b =>
                {
                    b.Property<int>("Id_Currency")
                        .HasColumnName("Id_currency")
                        .HasColumnType("int");

                    b.Property<decimal>("Exchange_Rate")
                        .HasColumnName("Exchange_rate")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id_Currency")
                        .HasName("PK__Currency");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("BankCore.Models.Loan", b =>
                {
                    b.Property<int>("Id_Loan")
                        .HasColumnName("Id_loan")
                        .HasColumnType("int");

                    b.Property<int>("Administrator")
                        .HasColumnName("Administrator")
                        .HasColumnType("int");

                    b.Property<int>("Bank_Account")
                        .HasColumnName("Bank_account")
                        .HasColumnType("int");

                    b.Property<int>("Client")
                        .HasColumnName("Client")
                        .HasColumnType("int");

                    b.Property<DateTime>("End_Of_Repayment")
                        .HasColumnName("End_of_repayment")
                        .HasColumnType("date");

                    b.Property<DateTime>("Granting_Date")
                        .HasColumnName("Granting_date")
                        .HasColumnType("date");

                    b.Property<decimal>("Installment")
                        .HasColumnName("Installment")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Installments_Count")
                        .HasColumnName("Installments_count")
                        .HasColumnType("numeric(3, 0)");

                    b.Property<decimal>("Outstanding_Amount")
                        .HasColumnName("Outstanding_amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Rate_Of_Interest")
                        .HasColumnName("Rate_of_interest")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<decimal>("Total_Amount")
                        .HasColumnName("Total_amount")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("Id_Loan")
                        .HasName("PK__Loan");

                    b.HasIndex("Administrator");

                    b.HasIndex("Bank_Account");

                    b.HasIndex("Client");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("BankCore.Models.Loan_Application", b =>
                {
                    b.Property<int>("Id_Loan_Application")
                        .HasColumnName("Id_loan_application")
                        .HasColumnType("int");

                    b.Property<int>("Administrator")
                        .HasColumnName("administrator")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnName("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("Bank_Account")
                        .HasColumnName("Bank_account")
                        .HasColumnType("int");

                    b.Property<int>("Client")
                        .HasColumnName("Client")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Decicion_Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Decision_date")
                        .HasColumnType("date")
                        .HasDefaultValue(null);

                    b.Property<decimal>("Installments_Count")
                        .HasColumnName("Installments_count")
                        .HasColumnType("numeric(3, 0)");

                    b.Property<decimal>("Repayment_Time")
                        .HasColumnName("Repayment_time")
                        .HasColumnType("numeric(3, 0)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<DateTime>("Submission_Date")
                        .HasColumnName("Submission_date")
                        .HasColumnType("date");

                    b.HasKey("Id_Loan_Application")
                        .HasName("PK__Loan_app");

                    b.HasIndex("Administrator");

                    b.HasIndex("Bank_Account");

                    b.HasIndex("Client");

                    b.ToTable("loan_applications");
                });

            modelBuilder.Entity("BankCore.Models.Transfer", b =>
                {
                    b.Property<int>("Id_Transfer")
                        .HasColumnName("Id_transfer")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnName("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Description")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<DateTime>("Execution_Date")
                        .HasColumnName("Execution_date")
                        .HasColumnType("date");

                    b.Property<string>("Receiver")
                        .IsRequired()
                        .HasColumnName("Receiver")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("Receiver_Bank_Account")
                        .HasColumnName("Receiver_bank_account")
                        .HasColumnType("int");

                    b.Property<int>("Sender_Bank_Account")
                        .HasColumnName("Sender_bank_account")
                        .HasColumnType("int");

                    b.Property<DateTime>("Sending_Date")
                        .HasColumnName("Sending_date")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("Title")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id_Transfer")
                        .HasName("PK__Transfer");

                    b.HasIndex("Receiver_Bank_Account");

                    b.HasIndex("Sender_Bank_Account");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("BankCore.Models.Administrator", b =>
                {
                    b.HasOne("BankCore.Models.Account", "IdAdministratorNavigation")
                        .WithOne("Administrator")
                        .HasForeignKey("BankCore.Models.Administrator", "Id_Administrator")
                        .HasConstraintName("FK__Administr__id_ad")
                        .IsRequired();
                });

            modelBuilder.Entity("BankCore.Models.Bank_Account", b =>
                {
                    b.HasOne("BankCore.Models.Client", "ClientNavigation")
                        .WithMany("BankAccount")
                        .HasForeignKey("Client")
                        .HasConstraintName("FK__Bank_acco__clien")
                        .IsRequired();

                    b.HasOne("BankCore.Models.Currency", "CurrencyNavigation")
                        .WithMany("BankAccount")
                        .HasForeignKey("Currency")
                        .HasConstraintName("FK__Bank_acco__curre")
                        .IsRequired();
                });

            modelBuilder.Entity("BankCore.Models.Client", b =>
                {
                    b.HasOne("BankCore.Models.Account", "IdClientNavigation")
                        .WithOne("Client")
                        .HasForeignKey("BankCore.Models.Client", "Id_Client")
                        .HasConstraintName("FK__Client__id_clien")
                        .IsRequired();
                });

            modelBuilder.Entity("BankCore.Models.Loan", b =>
                {
                    b.HasOne("BankCore.Models.Administrator", "AdministratorNavigation")
                        .WithMany("Loan")
                        .HasForeignKey("Administrator")
                        .HasConstraintName("FK__Loan__administra")
                        .IsRequired();

                    b.HasOne("BankCore.Models.Bank_Account", "BankAccountNavigation")
                        .WithMany("Loan")
                        .HasForeignKey("Bank_Account")
                        .HasConstraintName("FK__Loan__bank_accou")
                        .IsRequired();

                    b.HasOne("BankCore.Models.Client", "ClientNavigation")
                        .WithMany("Loan")
                        .HasForeignKey("Client")
                        .HasConstraintName("FK__Loan__client")
                        .IsRequired();
                });

            modelBuilder.Entity("BankCore.Models.Loan_Application", b =>
                {
                    b.HasOne("BankCore.Models.Administrator", "AdministratorNavigation")
                        .WithMany("LoanApplication")
                        .HasForeignKey("Administrator")
                        .HasConstraintName("FK__Loan_appl__admin")
                        .IsRequired();

                    b.HasOne("BankCore.Models.Bank_Account", "BankAccountNavigation")
                        .WithMany("LoanApplication")
                        .HasForeignKey("Bank_Account")
                        .HasConstraintName("FK__Loan_appl__bank")
                        .IsRequired();

                    b.HasOne("BankCore.Models.Client", "ClientNavigation")
                        .WithMany("LoanApplication")
                        .HasForeignKey("Client")
                        .HasConstraintName("FK__Loan_appl__clien__32767D0B")
                        .IsRequired();
                });

            modelBuilder.Entity("BankCore.Models.Transfer", b =>
                {
                    b.HasOne("BankCore.Models.Bank_Account", "ReceiverBankAccountNavigation")
                        .WithMany("TransferReceiverBankAccountNavigation")
                        .HasForeignKey("Receiver_Bank_Account")
                        .HasConstraintName("FK__Transfer__receiv")
                        .IsRequired();

                    b.HasOne("BankCore.Models.Bank_Account", "SenderBankAccountNavigation")
                        .WithMany("TransferSenderBankAccountNavigation")
                        .HasForeignKey("Sender_Bank_Account")
                        .HasConstraintName("FK__Transfer__sender")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}