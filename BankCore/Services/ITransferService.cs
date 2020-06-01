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
        Task<bool> CancelTransaction(TransferDto transferDto, CancellationToken cancellationToken);     

        Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account, int takeCount, int skipCount, CancellationToken cancellationToken);

        Task<IEnumerable<CurrencyDto>> ShowCurrencies(CancellationToken cancellationToken);

        Task<object> GetTransfer(int transferId, CancellationToken cancellationToken);

        Task<IEnumerable<TransferDto>> GetTransfers(int bankAccountId, CancellationToken cancellationToken);

        Task<IEnumerable<TransferDto>> GetAdminTransfers(string login, DateTime sendingDate, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<Transfer>>> ShowAwaitingTransfers(int takeCount, int skipCount, CancellationToken cancellationToken);

        Task<bool> CreateTransfer(TransferDto transferDto, string login, CancellationToken cancellationToken); 

        Task<bool> ExchangeMoney(CurrencyExchangeDto currencyExchangeDto, CancellationToken cancellationToken); 

        Task<bool> MakeTransfers(CancellationToken cancellationToken);
    }
}
