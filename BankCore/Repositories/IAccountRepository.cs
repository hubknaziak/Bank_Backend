using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> CreateClientAccount(Account user, Client client, CancellationToken cancellationToken);

        Task<bool> CreateAdminAccount(Account user, Administrator admin, CancellationToken cancellationToken);

        Task<bool> VerifyClientPassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> VerifyAdminPassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> ModifyAccount(CreateAccountDto modifyAccountDto, CancellationToken cancellationToken);

        Task<bool> ChangePassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> BlockAccount(string login, CancellationToken cancellationToken);

        Task<bool> UnblockAccount(string login, CancellationToken cancellationToken);

        //Task<object> GetAccount(string login, CancellationToken cancellationToken);

        Task<object> DeleteAccount(string login, CancellationToken cancellationToken);

    }
}
