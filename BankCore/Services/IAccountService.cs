using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;

namespace BankCore.Services
{
    public interface IAccountService
    {
        Task<bool> CreateClientAccount(CreateAccountDto CreateuserDto, CancellationToken cancellationToken);

        Task<bool> CreateAdminAccount(CreateAccountDto CreateuserDto, CancellationToken cancellationToken);

        Task<bool> VerifyClientPassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> VerifyAdminPassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> ModifyAccount(CreateAccountDto modifyAccountDto, CancellationToken cancellationToken);

        Task<bool> ChangePassword(AccountDto user, CancellationToken cancellationToken);

        Task<bool> BlockAccount(string login, CancellationToken cancellationToken);

        Task<bool> UnblockAccount(string login, CancellationToken cancellationToken);

        //Task<object> GetAccount(string login, CancellationToken cancellationToken);

        Task<object> DeleteAccount(string login, CancellationToken cancellationToken);

        string GenerateJwt(AccountDto user);
    }
}
