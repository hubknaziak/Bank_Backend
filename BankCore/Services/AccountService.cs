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

        public async Task<string> CreateClientAccount(CreateAccountDto createUserDto, CancellationToken cancellationToken)
        {
            if(createUserDto.AccountDto.Password == null)
            {
                return "null";
            }
            return await repository.CreateClientAccount(new Account
            {
                //Login = createUserDto.AccountDto.Login,
                Password = Crypto.HashPassword(createUserDto.AccountDto.Password),
                First_name = createUserDto.AccountDto.First_name,
                Last_name = createUserDto.AccountDto.Last_name
            }, new Client
            {
                Phone_Number = createUserDto.ClientDto.Phone_Number,
                Address = createUserDto.ClientDto.Address,
                Status = "Active"
            }, cancellationToken) 
            ;
        }

        public async Task<string> CreateAdminAccount(CreateAccountDto createUserDto, CancellationToken cancellationToken)
        {

            if (createUserDto.AccountDto.Password == null)
            {
                return "null";
            }
            return await repository.CreateAdminAccount(new Account
            {
                //Login = createUserDto.AccountDto.Login,
                Password = Crypto.HashPassword(createUserDto.AccountDto.Password),
                First_name = createUserDto.AccountDto.First_name,
                Last_name = createUserDto.AccountDto.Last_name
            }, new Administrator
            {
                Employment_Date = createUserDto.AdministratorDto.Employment_Date,
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

        public async Task<Tuple<int, IEnumerable<Account>>> ShowAllAccounts(int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            return await repository.ShowAllAccounts(takeCount, skipCount, cancellationToken);
            // var loan_Applications = await repository.ShowLoanApplications(takeCount, skipCount, administrator, cancellationToken);
            // return Tuple.Create(loan_Applications.Item1, loan_Applications.Item2.Select(x => mapper.Map<NoteDto>(x)));
        }

        public async Task<bool> ModifyAccount(CreateAccountDto modifyAccountDto, CancellationToken cancellationToken)
        {
            return await repository.ModifyAccount(modifyAccountDto, cancellationToken);
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

        public async Task<object> GetAccount(int id_Account, CancellationToken cancellationToken)
        {
            var account = await repository.GetAccount(id_Account, cancellationToken);
            return account;
        }

        public async Task<object> GetClientAccount(string login, CancellationToken cancellationToken)
        {
            var account = await repository.GetClientAccount(login, cancellationToken);
            return account;
        }

        public async Task<object> GetAdminAccount(string login, CancellationToken cancellationToken)
        {
            var account = await repository.GetAdminAccount(login, cancellationToken);
            return account;
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
                    new Claim(ClaimTypes.UserData, accountDto.Login) 
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
