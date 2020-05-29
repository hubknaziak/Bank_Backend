using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BankAPI.Extentions;
using System.Threading;
using BankCore;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Filters
{
    public class ValidateUserFilter : IValidateUserFilter
    {
        private readonly DatabaseContext context;

        public ValidateUserFilter(DatabaseContext context) => this.context = context;

        public async Task<string> ValidateUser(string login, CancellationToken cancellationToken)
        {
            string result;

            var account = await context.Accounts
             .SingleOrDefaultAsync(x => x.Login == login,
                 cancellationToken);

            var admin = await context.Administrators
                .SingleOrDefaultAsync(x => x.Id_Administrator == account.Id_account,
                cancellationToken);

            var client = await context.Clients
               .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account,
               cancellationToken);

            if (admin != null)
            {
                result = "admin";
                return result;
            }
            else if(client != null)
            {
                result = "client";
                return result;
            }
            else
            {
                result = "null";
                return result;
            }
        }
    }
}
