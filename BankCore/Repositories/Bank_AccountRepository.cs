using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BankCore.Repositories
{
    public class Bank_AccountRepository : IBank_AccountRepository
    {
        private readonly DatabaseContext context;

        public Bank_AccountRepository(DatabaseContext context) => this.context = context;

        public async Task<bool> BlockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == Id_Bank_Account, cancellationToken);

            record.Status = "blocked";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == Id_Bank_Account, cancellationToken);

            record.Status = "active";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<decimal> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == Id_Bank_Account, cancellationToken);

            return record.Account_Balance;
        }

        public async Task<bool> CreateBankAccount(Bank_Account bank_Account, Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == bank_AccountDto.ClientLogin, cancellationToken);

            if(record == null)
            {
                return false;
            }

            var currency = await context.Currencies
               .SingleOrDefaultAsync(x => x.Id_Currency == bank_AccountDto.Currency, cancellationToken);

            if (currency == null)
            {
                return false;
            }

            bank_Account.Client = record.Id_account;
            context.Bank_Accounts.Add(bank_Account);
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Tuple<int, IEnumerable<Bank_Account>>> ShowBankAccounts(int takeCount, int skipCount, int client, CancellationToken cancellationToken)
        {
            var count = await context.Bank_Accounts
              .CountAsync(x => x.Client == client);

            var bankAccounts = await context.Bank_Accounts.Where(x => x.Client == client)
                .OrderByDescending(x => x.Opening_Date)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, bankAccounts as IEnumerable<Bank_Account>);
        }
    }
}
