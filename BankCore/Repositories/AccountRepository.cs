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

namespace BankCore.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext context;

        public AccountRepository(DatabaseContext context) => this.context = context;

        public async Task<bool> CreateClientAccount(Account account, Client client, CancellationToken cancellationToken)
        {
            var record = context.Accounts
             .OrderByDescending(x => x.Id_account).FirstOrDefault();

            if (account == null) account.Id_account = 0;
            else account.Id_account = record.Id_account + 1;

            client.Id_Client = account.Id_account;
            context.Accounts.Add(account);
            context.Clients.Add(client);
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> CreateAdminAccount(Account account, Administrator admin, CancellationToken cancellationToken)
        {
            var record = context.Accounts
             .OrderByDescending(x => x.Id_account).FirstOrDefault();

            account.Id_account = record.Id_account + 1;
            admin.Id_Administrator = account.Id_account;
            context.Accounts.Add(account);
            context.Administrators.Add(admin);
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> VerifyPassword(AccountDto accountDto, CancellationToken cancellationToken)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountDto.Login)); 
            identity.AddClaim(new Claim(ClaimTypes.Name, accountDto.Login));

            var redirect = string.Empty;

            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == accountDto.Login, cancellationToken);

            if(record == null)
            {
                return "null";
            }

            var verify = Crypto.VerifyHashedPassword(record.Password, accountDto.Password);

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

                if (status != "Active")
                {
                    //ModelState.AddModelError("Login", "Account is inactive!");
                    //return Page();
                    return "null";
                }

                identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
                redirect = "Administrator";
            }
            else
            {
                var status = await context.Clients
                    .Where(x => x.Id_Client == record.Id_account)
                    .Select(x => x.Status)
                    .SingleAsync();

                if (status != "active")
                {
                    //ModelState.AddModelError("Login", "Account is inactive!");
                    //return Page();
                    return "null";
                }

                identity.AddClaim(new Claim(ClaimTypes.Role, "Client"));
                redirect = "Client";
            }

            var principal = new ClaimsPrincipal(identity);
            // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            //new AuthenticationProperties { IsPersistent = false });
            //return RedirectToPage(redirect);
            return redirect;

        }

        public async Task<Tuple<int, IEnumerable<Account>>> ShowAllAccounts(int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            var count = await context.Clients
               .CountAsync();

            var clients = await context.Clients.ToArrayAsync(cancellationToken);

            List<Account> accounts = null;

            for(int i = 0; i < count; i++)
            {
                accounts = await context.Accounts.Where(x => x.Id_account == clients[i].Id_Client)
                    .OrderByDescending(x => x.Id_account)
                    .Skip(skipCount)
                    .Take(takeCount)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
          

            return Tuple.Create(count, accounts as IEnumerable<Account>);
        }

        /*public async Task<bool> VerifyPassword(AccountDto accountDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == accountDto.Login, cancellationToken);

            var client = await context.Accounts
                .SingleOrDefaultAsync(x => x.Id_account == record.Id_account, cancellationToken);

            if(client != null)
            {
                return record != null &&
               Crypto.VerifyHashedPassword(record.Password, accountDto.Password);
            }
            else
            {
                return false;
            }
        }*/

        /* public async Task<bool> VerifyAdminPassword(AccountDto accountDto, CancellationToken cancellationToken)
         {
             var record = await context.Accounts
                 .SingleOrDefaultAsync(x => x.Login == accountDto.Login, cancellationToken);

             var admin = await context.Accounts
                .SingleOrDefaultAsync(x => x.Id_account == record.Id_account, cancellationToken);

             if (admin != null)
             {
                 return record != null &&
                Crypto.VerifyHashedPassword(record.Password, accountDto.Password);
             }
             else
             {
                 return false;
             }
         }*/

        public async Task<bool> ModifyAccount(CreateAccountDto modifyAccountDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == modifyAccountDto.AccountDto.Login, cancellationToken);

            var client = await context.Clients
               .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            if (!Crypto.VerifyHashedPassword(record.Password, modifyAccountDto.AccountDto.Password))
            {
                return false;
            }

            //record.Password = Crypto.HashPassword(modifyAccountDto.AccountDto.NewPassword);
            record.Login = modifyAccountDto.AccountDto.NewLogin;
            record.First_name = modifyAccountDto.AccountDto.NewFirst_name;
            record.Last_name = modifyAccountDto.AccountDto.NewLastName;
            client.Phone_Number = modifyAccountDto.ClientDto.NewPhone_Number;
            client.Address = modifyAccountDto.ClientDto.NewAddress;
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> ChangePassword(AccountDto accountDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == accountDto.Login, cancellationToken);

            if (!Crypto.VerifyHashedPassword(record.Password, accountDto.Password))
            {
                return false;
            }

            record.Password = Crypto.HashPassword(accountDto.NewPassword);
    
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> BlockAccount(string login,  CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            var client = await context.Clients
                .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            client.Status = "Inactive";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UnblockAccount(string login, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
                .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            var client = await context.Clients
                .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            client.Status = "Active";

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }


        public async Task<object> GetAccount(int id_Account, 
            CancellationToken cancellationToken)
        {
            var account = await context.Accounts
              .SingleOrDefaultAsync(x => x.Id_account == id_Account,
                  cancellationToken);

            if (account == null)
            {
                return null;
            }
           
            return account;
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
