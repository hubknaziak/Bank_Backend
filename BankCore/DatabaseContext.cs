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

        public DbSet<Transfer> Transfers  { get; set; }

        public DbSet<Loan_Application> Loan_Applications { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) 
        { 
        }
    }
}
