using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;
using BankCore.Repositories;
using CryptoHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository repository;

        private readonly IConfiguration configuration;

        private readonly string secretKey;

        public AccountService(IAccountRepository repository, IConfiguration configuration,
            string secretKey)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.secretKey = secretKey;
        }

        public async Task<string> CreateClientAccount(ClientDto clientDto, CancellationToken cancellationToken)
        {
            if(clientDto.password == null)
            {
                return "null";
            }
            return await repository.CreateClientAccount(new Account
            {
                //Login = createUserDto.AccountDto.Login,
                Password = Crypto.HashPassword(clientDto.password),
                First_name = clientDto.firstName,
                Last_name = clientDto.lastName
            }, new Client
            {
                Phone_Number = clientDto.phoneNumber,
                Address = clientDto.address,
                Status = "Active"
            }, cancellationToken) 
            ;
        }

        public async Task<string> CreateAdminAccount(AdministratorDto administratorDto, CancellationToken cancellationToken)
        {

            if (administratorDto.password == null)
            {
                return "null";
            }
            return await repository.CreateAdminAccount(new Account
            {
                //Login = createUserDto.AccountDto.Login,
                Password = Crypto.HashPassword(administratorDto.password),
                First_name = administratorDto.firstName,
                Last_name = administratorDto.lastName
            }, new Administrator
            {
                Employment_Date = administratorDto.employmentDate,
                Status = "Active"
            }, cancellationToken)
            ;
        }

        public async Task<string> VerifyPassword(AccountDto userDto, CancellationToken cancellationToken)
        {
            return await repository.VerifyPassword(userDto, cancellationToken);
        }

        /*public async Task<bool> VerifyAdminPassword(AccountDto userDto, CancellationToken cancellationToken)
        {
            return await repository.VerifyAdminPassword(userDto, cancellationToken);
        }*/

        public async Task<IEnumerable<GetClientDto>> ShowAllAccounts(CancellationToken cancellationToken)
        {
            return await repository.ShowAllAccounts( cancellationToken);
        }

        public async Task<bool> ModifyAccount(GetClientDto clientDto, CancellationToken cancellationToken)
        {
            return await repository.ModifyAccount(clientDto, cancellationToken);
        }

        public async Task<bool> ChangePassword(AccountDto userDto, CancellationToken cancellationToken)
        {
            return await repository.ChangePassword(userDto, cancellationToken);
        }

        public async Task<bool> BlockAccount(string login, CancellationToken cancellationToken)
        {
            return await repository.BlockAccount(login, cancellationToken);
        }

        public async Task<bool> UnblockAccount(string login, CancellationToken cancellationToken)
        {
            return await repository.UnblockAccount(login, cancellationToken);
        }

        public async Task<string> GetAccountType(string login, CancellationToken cancellationToken)
        {
            return await repository.GetAccountType(login, cancellationToken);
        }

        public async Task<object> GetClientAccount(string login, CancellationToken cancellationToken)
        {
            return await repository.GetClientAccount(login, cancellationToken);
        }

        public async Task<object> GetAdminAccount(string login, CancellationToken cancellationToken)
        {
            return await repository.GetAdminAccount(login, cancellationToken);
        }

        public async Task<object> DeleteAccount(string login,
            CancellationToken cancellationToken)
        {
            return await repository.DeleteAccount(login, cancellationToken);
        }

        public string GenerateJwt(AccountDto accountDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.UserData, accountDto.login) 
                }),
                Expires = DateTime.UtcNow.AddHours(configuration.GetValue<int>("TokenExpiryHours")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
