using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;
using BankCore.Repositories;

namespace BankCore.Services
{
    public class Bank_AccountService : IBank_AccountService
    {
        private readonly IBank_AccountRepository repository;

        public Bank_AccountService(IBank_AccountRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> BlockBankAccount(Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken)
        {
            return await repository.BlockBankAccount(bank_AccountDto, cancellationToken);
        }

        public async Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            return await repository.UnblockBankAccount(Id_Bank_Account, cancellationToken);
        }

        public async Task<decimal> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            return await repository.CheckAccountAmount(Id_Bank_Account, cancellationToken);
        }

        public async Task<object> CreateBankAccount(string login, Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken)
        {
            return await repository.CreateBankAccount(new Bank_Account
            {
                Opening_Date = DateTime.Now,
                Account_Balance = bank_AccountDto.balance,
                Currency = bank_AccountDto.currencyId,
                Status = "active"
            }, bank_AccountDto, login, cancellationToken);
        }

        public async Task<bool> DeleteBankAccount(int id, CancellationToken cancellationToken)
        {
            return await repository.DeleteBankAccount(id, cancellationToken);
        }

        public async Task< IEnumerable<Bank_AccountDto>> ShowBankAccounts(string login, CancellationToken cancellationToken)
        {
            return await repository.ShowBankAccounts(login, cancellationToken);
        }
    }
}
