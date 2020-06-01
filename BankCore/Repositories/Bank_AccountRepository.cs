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

        public async Task<bool> BlockBankAccount(Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == bank_AccountDto.bankAccountId, cancellationToken);

            if (record == null)
            {
                return false;
            }

            record.Status = bank_AccountDto.status;

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == Id_Bank_Account, cancellationToken);

            if (record == null)
            {
                return false;
            }

            record.Status = "active";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<decimal> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == Id_Bank_Account, cancellationToken);

            if (record == null)
            {
                return 0;
            }

            return record.Account_Balance;
        }

        public async Task<bool> DeleteBankAccount(int id, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == id, cancellationToken);

            if (record == null)
            {
                return false;
            }

            context.Remove(record);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<object> CreateBankAccount(Bank_Account bank_Account, Bank_AccountDto bank_AccountDto, string login, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            if(record == null)
            {
                return false;
            }

            var client = await context.Clients
               .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            if (client == null)
            {
                return false;
            }

            var currency = await context.Currencies
               .SingleOrDefaultAsync(x => x.Id_Currency == bank_AccountDto.currencyId, cancellationToken);

            if (currency == null)
            {
                return false;
            }

            bank_Account.Client = record.Id_account;

            var bankId = context.Bank_Accounts
             .OrderByDescending(x => x.Id_Bank_Account).FirstOrDefault();

            bank_AccountDto.bankAccountId = bankId.Id_Bank_Account + 1;
            bank_AccountDto.currencyName = currency.Name;
            bank_AccountDto.status = bank_Account.Status;


            context.Bank_Accounts.Add(bank_Account);
            try
            {
               await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return bank_AccountDto;
        }

        public async Task<IEnumerable<Bank_AccountDto>> ShowBankAccounts(string login, CancellationToken cancellationToken)
        {
            var account = await context.Accounts
             .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            if (account == null)
            {
                return null;
            }

            var client = await context.Clients
             .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account, cancellationToken);

            var count = await context.Bank_Accounts
              .CountAsync(x => x.Client == client.Id_Client);

            var bankAccounts = await context.Bank_Accounts.Where(x => x.Client == client.Id_Client)
                .OrderByDescending(x => x.Opening_Date)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            Bank_AccountDto bank_AccountDto = new Bank_AccountDto();
            Bank_AccountDto[] bank_AccountsDto = new Bank_AccountDto[count];
            int i = 0;

            foreach(Bank_Account b in bankAccounts)
            {
                bank_AccountDto = new Bank_AccountDto();
                bank_AccountDto.bankAccountId = b.Id_Bank_Account;
                bank_AccountDto.balance = b.Account_Balance;
                bank_AccountDto.currencyId = b.Currency;

                var currency = await context.Currencies
            .SingleOrDefaultAsync(x => x.Id_Currency == b.Currency, cancellationToken);

                bank_AccountDto.currencyName = currency.Name;
                bank_AccountDto.status = b.Status;
                bank_AccountsDto[i] = bank_AccountDto;
                i++;
            }

            return bank_AccountsDto as IEnumerable<Bank_AccountDto>;
        }
    }
}
