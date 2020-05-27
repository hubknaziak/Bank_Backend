using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Models;

namespace BankCore.Repositories
{
    public interface ITransferRepository
    {
        Task<bool> CancelTransaction(int Id_Transfer, CancellationToken cancellationToken); //DONE

        Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account, int takeCount, int skipCount, CancellationToken cancellationToken); //DONE

        Task<Tuple<int, IEnumerable<Currency>>> ShowCurrencies(int takeCount, int skipCount, CancellationToken cancellationToken); //DONE

        Task<Tuple<int, IEnumerable<Transfer>>> ShowAwaitingTransfers(int takeCount, int skipCount, CancellationToken cancellationToken); //DONE

        Task<bool> CreateTransfer(Transfer transfer, CancellationToken cancellationToken);      //DONE

        Task<bool> MakeTransfers(CancellationToken cancellationToken);      //DONE
    }
}
