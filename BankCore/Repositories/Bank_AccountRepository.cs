﻿using System;
using System.Collections.Generic;
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

        public async Task<Bank_Account> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            var record = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == Id_Bank_Account, cancellationToken);

            return record;
        }

        public async Task<bool> CreateBankAccount(Bank_Account bank_Account, Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == bank_AccountDto.ClientLogin, cancellationToken);

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
    }
}