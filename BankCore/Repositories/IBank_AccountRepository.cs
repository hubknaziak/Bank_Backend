using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Repositories
{
    public interface IBank_AccountRepository
    {
        Task<bool> CreateBankAccount(Bank_Account bank_Account, Bank_AccountDto bank_AccountDto, CancellationToken cancellationToken); //DONE

        Task<bool> BlockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken);  //DONE

        Task<bool> UnblockBankAccount(int Id_Bank_Account, CancellationToken cancellationToken);    //DONE

        Task<decimal> CheckAccountAmount(int Id_Bank_Account, CancellationToken cancellationToken);    //DONE

        Task<Tuple<int, IEnumerable<Bank_Account>>> ShowBankAccounts(int takeCount, int skipCount, int client, CancellationToken cancellationToken);   //DONE
    }
}
