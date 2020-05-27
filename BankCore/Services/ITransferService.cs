using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Services
{
    public interface ITransferService
    {
        Task<bool> CancelTransaction(int Id_Transfer, CancellationToken cancellationToken);     //update

        Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account, int takeCount, int skipCount, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<Currency>>> ShowCurrencies(int takeCount, int skipCount, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<Transfer>>> ShowAwaitingTransfers(int takeCount, int skipCount, CancellationToken cancellationToken);

        Task<bool> CreateTransfer(TransferDto transferDto, CancellationToken cancellationToken); //create

        Task<bool> MakeTransfers(CancellationToken cancellationToken);
    }
}
