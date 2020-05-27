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
    [Migration("20200519111623_Bank_Account")]
    partial class Bank_Account
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
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id_account");

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

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Openinig_Date")
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
#pragma warning restore 612, 618
        }
    }
}