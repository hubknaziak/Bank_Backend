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
            //this.configuration = configuration;
            // this.secretKey = secretKey;
        }

        public async Task<bool> BlockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            return await repository.BlockBankAccount(Id_Bank_Account, cancellationToken);
        }

        public async Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            return await repository.UnblockBankAccount(Id_Bank_Account, cancellationToken);
        }

        public async Task<Bank_Account> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken)
        {
            return await repository.CheckAccountAmount(Id_Bank_Account, cancellationToken);
        }

        public async Task<bool> CreateBankAccount(Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken)
        {
            return await repository.CreateBankAccount(new Bank_Account
            {
                Openinig_Date = DateTime.Now,
                Account_Balance = decimal.Zero,
                Currency = bank_AccountDto.Currency,
                Status = "active"
            }, bank_AccountDto, cancellationToken);
        }
    }
}
