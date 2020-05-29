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
    [Migration("20200528230014_Initial")]
    partial class Initial
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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("First_name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Last_name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("varchar(9) CHARACTER SET utf8mb4")
                        .HasMaxLength(9);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id_account");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankCore.Models.Administrator", b =>
                {
                    b.Property<int>("Id_Administrator")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Employment_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id_Administrator");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("BankCore.Models.Bank_Account", b =>
                {
                    b.Property<int>("Id_Bank_Account")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Account_Balance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Client")
                        .HasColumnType("int");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime>("Opening_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id_Bank_Account");

                    b.ToTable("Bank_Accounts");
                });

            modelBuilder.Entity("BankCore.Models.Client", b =>
                {
                    b.Property<int>("Id_Client")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Phone_Number")
                        .IsRequired()
                        .HasColumnType("varchar(12) CHARACTER SET utf8mb4")
                        .HasMaxLength(12);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id_Client");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("BankCore.Models.Currency", b =>
                {
                    b.Property<int>("Id_Currency")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Exchange_Rate")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.HasKey("Id_Currency");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("BankCore.Models.Loan", b =>
                {
                    b.Property<int>("Id_Loan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Administrator")
                        .HasColumnType("int");

                    b.Property<int>("Bank_Account")
                        .HasColumnType("int");

                    b.Property<int>("Client")
                        .HasColumnType("int");

                    b.Property<DateTime>("End_Of_Repayment")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Granting_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Installment")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Installments_Count")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Outstanding_Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Rate_Of_Interest")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<decimal>("Total_Amount")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id_Loan");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("BankCore.Models.Loan_Application", b =>
                {
                    b.Property<int>("Id_Loan_Application")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Administrator")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Bank_Account")
                        .HasColumnType("int");

                    b.Property<int>("Client")
                        .HasColumnType("int");

                    b.Property<DateTime>("Decicion_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Installments_Count")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("Repayment_Time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<DateTime>("Submission_Date")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id_Loan_Application");

                    b.ToTable("Loan_Applications");
                });

            modelBuilder.Entity("BankCore.Models.Transfer", b =>
                {
                    b.Property<int>("Id_Transfer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Execution_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Receiver")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<int>("Receiver_Bank_Account")
                        .HasColumnType("int");

                    b.Property<int>("Sender_Bank_Account")
                        .HasColumnType("int");

                    b.Property<DateTime>("Sending_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id_Transfer");

                    b.ToTable("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}