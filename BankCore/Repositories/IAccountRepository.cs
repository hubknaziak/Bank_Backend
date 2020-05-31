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
        Task<string> CreateClientAccount(Account user, Client client, CancellationToken cancellationToken);

        Task<string> CreateAdminAccount(Account user, Administrator admin, CancellationToken cancellationToken);

        Task<string> VerifyPassword(AccountDto user, CancellationToken cancellationToken);

        //Task<bool> VerifyAdminPassword(AccountDto user, CancellationToken cancellationToken);

        Task<IEnumerable<GetClientDto>> ShowAllAccounts(CancellationToken cancellationToken);   //DONE

        Task<bool> ModifyAccount(GetClientDto clientDto, CancellationToken cancellationToken);

        Task<bool> ChangePassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> BlockAccount(string login, CancellationToken cancellationToken);

        Task<bool> UnblockAccount(string login, CancellationToken cancellationToken);

        Task<string> GetAccountType(string login, CancellationToken cancellationToken);

        Task<object> GetClientAccount(string login, CancellationToken cancellationToken);

        Task<object> GetAdminAccount(string login, CancellationToken cancellationToken);

        Task<object> DeleteAccount(string login, CancellationToken cancellationToken);

    }
}
