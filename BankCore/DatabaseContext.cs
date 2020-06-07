using BankCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BankCore
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Administrator> Administrators { get; set; }

        public DbSet<Bank_Account> Bank_Accounts { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<Loan_Application> Loan_Applications { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id_account)
                   .HasName("PK__Account");

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Account")
                    .IsUnique();

                entity.Property(e => e.Id_account)
                    .HasColumnName("Id_account")
                    .ValueGeneratedNever();

                entity.Property(e => e.First_name)
                    .IsRequired()
                    .HasColumnName("First_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Last_name)
                    .IsRequired()
                    .HasColumnName("Last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("Login")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("Password")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.Id_Administrator)
                    .HasName("PK__Administ");

                entity.Property(e => e.Id_Administrator)
                    .HasColumnName("Id_administrator")
                    .ValueGeneratedNever();

                entity.Property(e => e.Employment_Date)
                    .HasColumnName("Employment_date")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAdministratorNavigation)
                    .WithOne(p => p.Administrator)
                    .HasForeignKey<Administrator>(d => d.Id_Administrator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Administr__id_ad");
            });

            modelBuilder.Entity<Bank_Account>(entity =>
            {
                entity.HasKey(e => e.Id_Bank_Account)
                    .HasName("PK__Bank_acc");

                entity.ToTable("bank_accounts");

                entity.Property(e => e.Id_Bank_Account)
                    .HasColumnName("Id_bank_account")
                    .ValueGeneratedNever();

                entity.Property(e => e.Account_Balance)
                    .HasColumnName("Account_balance")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Client).HasColumnName("Client");

                entity.Property(e => e.Currency).HasColumnName("Currency");

                entity.Property(e => e.Opening_Date)
                    .HasColumnName("Opening_date")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.BankAccount)
                    .HasForeignKey(d => d.Client)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bank_acco__clien");

                entity.HasOne(d => d.CurrencyNavigation)
                    .WithMany(p => p.BankAccount)
                    .HasForeignKey(d => d.Currency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bank_acco__curre");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id_Client)
                   .HasName("PK__Client");

                entity.Property(e => e.Id_Client)
                    .HasColumnName("Id_client")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone_Number)
                    .IsRequired()
                    .HasColumnName("Phone_number")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClientNavigation)
                    .WithOne(p => p.Client)
                    .HasForeignKey<Client>(d => d.Id_Client)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__id_clien");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Id_Currency)
                    .HasName("PK__Currency");

                entity.Property(e => e.Id_Currency)
                    .HasColumnName("Id_currency")
                    .ValueGeneratedNever();

                entity.Property(e => e.Exchange_Rate)
                    .HasColumnName("Exchange_rate")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("Name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.Id_Loan)
                    .HasName("PK__Loan");

                entity.Property(e => e.Id_Loan)
                    .HasColumnName("Id_loan")
                    .ValueGeneratedNever();

                entity.Property(e => e.Administrator).HasColumnName("Administrator");

                entity.Property(e => e.Bank_Account).HasColumnName("Bank_account");

                entity.Property(e => e.Client).HasColumnName("Client");

                entity.Property(e => e.End_Of_Repayment)
                    .HasColumnName("End_of_repayment")
                    .HasColumnType("date");

                entity.Property(e => e.Granting_Date)
                    .HasColumnName("Granting_date")
                    .HasColumnType("date");

                entity.Property(e => e.Installment)
                    .HasColumnName("Installment")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Installments_Count)
                    .HasColumnName("Installments_count")
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.Outstanding_Amount)
                    .HasColumnName("Outstanding_amount")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Rate_Of_Interest)
                    .HasColumnName("Rate_of_interest")
                    .HasColumnType("decimal(4, 2)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Total_Amount)
                    .HasColumnName("Total_amount")
                    .HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.AdministratorNavigation)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.Administrator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loan__administra");

                entity.HasOne(d => d.BankAccountNavigation)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.Bank_Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loan__bank_accou");

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.Client)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loan__client");
            });

            modelBuilder.Entity<Loan_Application>(entity =>
            {
                entity.HasKey(e => e.Id_Loan_Application)
                    .HasName("PK__Loan_app");

                entity.ToTable("loan_applications");

                entity.Property(e => e.Id_Loan_Application)
                    .HasColumnName("Id_loan_application")
                    .ValueGeneratedNever();

                entity.Property(e => e.Administrator).HasColumnName("administrator");

                entity.Property(e => e.Amount)
                    .HasColumnName("Amount")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Bank_Account).HasColumnName("Bank_account");

                entity.Property(e => e.Client).HasColumnName("Client");

                entity.Property(e => e.Decicion_Date)
                    .HasColumnName("Decision_date")
                    .HasDefaultValue(null)
                    .HasColumnType("date");

                entity.Property(e => e.Installments_Count)
                    .HasColumnName("Installments_count")
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.Repayment_Time)
                    .HasColumnName("Repayment_time")
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Submission_Date)
                    .HasColumnName("Submission_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.AdministratorNavigation)
                    .WithMany(p => p.LoanApplication)
                    .HasForeignKey(d => d.Administrator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loan_appl__admin");

                entity.HasOne(d => d.BankAccountNavigation)
                    .WithMany(p => p.LoanApplication)
                    .HasForeignKey(d => d.Bank_Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loan_appl__bank");

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.LoanApplication)
                    .HasForeignKey(d => d.Client)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loan_appl__clien__32767D0B");
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasKey(e => e.Id_Transfer)
                    .HasName("PK__Transfer");

                entity.Property(e => e.Id_Transfer)
                    .HasColumnName("Id_transfer")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount)
                    .HasColumnName("Amount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Execution_Date)
                    .HasColumnName("Execution_date")
                    .HasColumnType("date");

                entity.Property(e => e.Receiver)
                    .IsRequired()
                    .HasColumnName("Receiver")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Receiver_Bank_Account).HasColumnName("Receiver_bank_account");

                entity.Property(e => e.Sender_Bank_Account).HasColumnName("Sender_bank_account");

                entity.Property(e => e.Sending_Date)
                    .HasColumnName("Sending_date")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("Title")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ReceiverBankAccountNavigation)
                    .WithMany(p => p.TransferReceiverBankAccountNavigation)
                    .HasForeignKey(d => d.Receiver_Bank_Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transfer__receiv");

                entity.HasOne(d => d.SenderBankAccountNavigation)
                    .WithMany(p => p.TransferSenderBankAccountNavigation)
                    .HasForeignKey(d => d.Sender_Bank_Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transfer__sender");
            });

           // OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
