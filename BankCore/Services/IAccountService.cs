﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Services
{
    public interface IAccountService
    {
        Task<string> CreateClientAccount(ClientDto clientDto, CancellationToken cancellationToken);

        Task<string> CreateAdminAccount(AdministratorDto administratorDto, CancellationToken cancellationToken);

        Task<string> VerifyPassword(AccountDto user, CancellationToken cancellationToken);


        Task<IEnumerable<GetClientDto>> ShowAllAccounts(CancellationToken cancellationToken);

        Task<bool> ModifyAccount(GetClientDto clientDto, CancellationToken cancellationToken);

        Task<bool> BlockAccount(string login, CancellationToken cancellationToken);

        Task<bool> UnblockAccount(string login, CancellationToken cancellationToken);

        Task<string> GetAccountType(string login, CancellationToken cancellationToken);

        Task<object> GetClientAccount(string login, CancellationToken cancellationToken);

        Task<object> GetAdminAccount(string login, CancellationToken cancellationToken);

        Task<object> DeleteAccount(string login, CancellationToken cancellationToken);

        string GenerateJwt(AccountDto accountDto);
    }
}
