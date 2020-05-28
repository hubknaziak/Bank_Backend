using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankAPI.Filters
{
    public interface IValidateUserFilter
    {
        Task<string> ValidateUser(string login, CancellationToken cancellationToken);
    }
}
