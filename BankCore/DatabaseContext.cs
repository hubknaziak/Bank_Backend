using BankCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BankCore
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Administrator> Administrators { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) 
        { 
        }
    }
}
