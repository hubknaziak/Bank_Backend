using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Models;

namespace BankCore.Repositories
{
    public interface IClientRepository
    {
        Task<bool> CreateAccount(Client client, CancellationToken cancellationToken);

        //Task<bool> VerifyPassword(AccountDto user, CancellationToken cancellationToken);

        //Task<bool> ModifyAccount(AccountDto user, CancellationToken cancellationToken);

        //Task<object> GetAccount(string login, CancellationToken cancellationToken);

       // Task<object> DeleteAccount(string login, CancellationToken cancellationToken);
    }
}
