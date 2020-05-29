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
        Task<bool> CreateBankAccount(Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken); //create

        Task<bool> BlockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken);  //update

        Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken);  //update

        Task<decimal> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken);    //get

        Task<Tuple<int, IEnumerable<Bank_Account>>> ShowBankAccounts(int takeCount, int skipCount, int client, CancellationToken cancellationToken);
    }
}
