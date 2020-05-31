using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Repositories
{
    public interface ITransferRepository
    {
        Task<bool> CancelTransaction(TransferDto transferDto, CancellationToken cancellationToken); //DONE

        Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account, int takeCount, int skipCount, CancellationToken cancellationToken); //DONE

        Task<IEnumerable<CurrencyDto>> ShowCurrencies( CancellationToken cancellationToken); //DONE

        Task<object> GetTransfer(int transferId, CancellationToken cancellationToken); //DONE

        Task<IEnumerable<TransferDto>> GetTransfers(int bankAccountId, CancellationToken cancellationToken); //DONE

        Task<IEnumerable<TransferDto>> GetAdminTransfers(string login, DateTime sendingDate, CancellationToken cancellationToken); //DONE


        Task<Tuple<int, IEnumerable<Transfer>>> ShowAwaitingTransfers(int takeCount, int skipCount, CancellationToken cancellationToken); //DONE

        Task<bool> CreateTransfer(Transfer transfer, CancellationToken cancellationToken);      //DONE

        Task<bool> ExchangeMoney(Transfer transfer, CancellationToken cancellationToken);      //DONE

        Task<bool> MakeTransfers(CancellationToken cancellationToken);      //DONE
    }
}
