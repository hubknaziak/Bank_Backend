using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CryptoHelper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankCore.Dtos;
using BankCore.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.AspNetCore.Http;

namespace BankCore.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext context;

        public AccountRepository(DatabaseContext context) => this.context = context;

        public async Task<string> CreateClientAccount(Account account, Client client, CancellationToken cancellationToken)
        {
            var record = context.Accounts
             .OrderByDescending(x => x.Id_account).FirstOrDefault();

            if (record == null) { account.Id_account = 0;}
            else { account.Id_account = record.Id_account + 1;}

            var newLogin = account.Id_account.ToString().PadLeft(9,'0');

            account.Login = newLogin;
            client.Id_Client = account.Id_account;
            context.Accounts.Add(account);
            context.Clients.Add(client);
            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return account.Login;
            }
            catch (DbUpdateException)
            {
                return "null";
            }
        }

        public async Task<string> CreateAdminAccount(Account account, Administrator admin, CancellationToken cancellationToken)
        {
            var record = context.Accounts
             .OrderByDescending(x => x.Id_account).FirstOrDefault();

            if (record == null) { account.Id_account = 0; account.Login = account.Id_account.ToString(); }
            else { account.Id_account = record.Id_account + 1; account.Login = account.Id_account.ToString(); }

            var newLogin = account.Id_account.ToString().PadLeft(9, '0');

            account.Login = newLogin;
            admin.Id_Administrator = account.Id_account;
            context.Accounts.Add(account);
            context.Administrators.Add(admin);
            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return account.Login;
            }
            catch (DbUpdateException)
            {
                return "null";
            }
        }

        public async Task<string> VerifyPassword(AccountDto accountDto, CancellationToken cancellationToken)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountDto.login)); 
            identity.AddClaim(new Claim(ClaimTypes.Name, accountDto.login));

            var redirect = string.Empty;

            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == accountDto.login, cancellationToken);

            if(record == null)
            {
                return "null";
            }

            var verify = Crypto.VerifyHashedPassword(record.Password, accountDto.password);

            if(!verify)
            {
                return "null";
            }

         
            var isAdmin = context.Administrators
            .Any(x => x.Id_Administrator == record.Id_account);

            if(isAdmin)
            {
               var status = await context.Administrators
                   .Where(x => x.Id_Administrator == record.Id_account)
                   .Select(x => x.Status)
                   .SingleAsync();

                if (status != "active")
                {
                    return "null";
                }

                identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
                redirect = "Admin";
            }
            else
            {
                var status = await context.Clients
                    .Where(x => x.Id_Client == record.Id_account)
                    .Select(x => x.Status)
                    .SingleAsync();

                if (status != "active")
                {
                    return "null";
                }

                identity.AddClaim(new Claim(ClaimTypes.Role, "Client"));
                redirect = "Client";
            }

            var principal = new ClaimsPrincipal(identity);
         
            return redirect;

        }

        public async Task<IEnumerable<GetClientDto>> ShowAllAccounts(CancellationToken cancellationToken)
        {
            var count = await context.Clients
               .CountAsync();

            Client[] clients = new Client[count];

            clients = await context.Clients.OrderByDescending(x => x.Id_Client)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            if(clients == null)
            {
                return null;
            }

            GetClientDto getClientDto = new GetClientDto();
            Account [] account = new Account[count];
            Account [] accounts = new Account[count];
            GetClientDto [] getClients = new GetClientDto[count];

            for (int i = 0; i < count; i++)
            {
                account = await context.Accounts.Where(x => x.Id_account == clients[i].Id_Client)
                    .ToArrayAsync(cancellationToken);
                accounts[i] = account[0];
            }


            for(int i = 0; i < count; i++)
            {
                getClientDto = new GetClientDto();
                getClientDto.login = accounts[i].Login;
                getClientDto.firstName = accounts[i].First_name;
                getClientDto.lastName = accounts[i].Last_name;
                getClientDto.status = clients[i].Status;
                getClientDto.phoneNumber = clients[i].Phone_Number;
                getClientDto.address = clients[i].Address;
                getClients[i] = getClientDto;
            }

            return  getClients as IEnumerable<GetClientDto>;
        }


        public async Task<bool> ModifyAccount(GetClientDto clientDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == clientDto.login, cancellationToken);

            if (record == null)
            {
                return false;
            }

            var client = await context.Clients
               .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            if (client == null)
            {
                return false;
            }

            if (clientDto.password != null) record.Password = Crypto.HashPassword(clientDto.password);
            record.First_name = clientDto.firstName;
            record.Last_name = clientDto.lastName;
            client.Status = clientDto.status;
            client.Phone_Number = clientDto.phoneNumber;
            client.Address = clientDto.address;
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> ChangePassword(AccountDto accountDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == accountDto.login, cancellationToken);

            if (!Crypto.VerifyHashedPassword(record.Password, accountDto.password))
            {
                return false;
            }
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> BlockAccount(string login,  CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            var client = await context.Clients
                .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            client.Status = "inactive";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UnblockAccount(string login, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            var client = await context.Clients
                .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            client.Status = "active";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }


        public async Task<string> GetAccountType(string login, 
            CancellationToken cancellationToken)
        {
            var account = await context.Accounts
              .SingleOrDefaultAsync(x => x.Login == login,
                  cancellationToken);

            if(account == null)
            {
                return "null";
            }

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account,
                  cancellationToken);

            var admin = await context.Administrators
              .SingleOrDefaultAsync(x => x.Id_Administrator == account.Id_account,
                  cancellationToken);

            if(client == null && admin != null)
            {
                return "Admin";
            }
            else if(client != null && admin == null)
            {
                return "Client";
            }
           
            return "null";
        }

        public async Task<object> GetClientAccount(string login, CancellationToken cancellationToken)
        {

            var account = await context.Accounts
              .SingleOrDefaultAsync(x => x.Login == login,
                  cancellationToken);

            if (account == null)
            {
                return null;
            }

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account,
                  cancellationToken);

            GetClientDto getClient = new GetClientDto
            {
                login = account.Login,
                firstName = account.First_name,
                lastName = account.Last_name,
                status = client.Status,
                phoneNumber = client.Phone_Number,
                address = client.Address
            };
             
            return getClient;
        }

        public async Task<object> GetAdminAccount(string login, CancellationToken cancellationToken)
        {
            var account = await context.Accounts
              .SingleOrDefaultAsync(x => x.Login == login,
                  cancellationToken);

            if (account == null)
            {
                return null;
            }


            var admin = await context.Administrators
              .SingleOrDefaultAsync(x => x.Id_Administrator == account.Id_account,
                  cancellationToken);

            GetAdminDto getAdmin = new GetAdminDto
            {
                login = account.Login,
                firstName = account.First_name,
                lastName = account.Last_name,
                status = admin.Status,
                employmentDate = admin.Employment_Date
            };

            return getAdmin;
        }

        public async Task<object> DeleteAccount(string login,
          CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            if (record == null)
            {
                return null;
            }

            context.Remove(record);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

    }
}
