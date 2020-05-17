using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;

namespace BankCore.Services
{
    public interface IClientService
    {
        Task<bool> CreateAccount(ClientDto clientDto, CancellationToken cancellationToken);
    }
}
