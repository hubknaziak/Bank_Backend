using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Services
{
    public interface IBank_AccountService
    {
        Task<object> CreateBankAccount(string login, Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken); //create

        Task<bool> BlockBankAccount(Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken);  //update

        Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken);  //update

        Task<decimal> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken);    //get

        Task<bool> DeleteBankAccount(int id, CancellationToken cancellationToken);

        Task< IEnumerable<Bank_AccountDto>> ShowBankAccounts(string login, CancellationToken cancellationToken);
    }
}
