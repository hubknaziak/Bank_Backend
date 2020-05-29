using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BankCore.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly DatabaseContext context;

        public TransferRepository(DatabaseContext context) => this.context = context;


        public async Task<bool> CancelTransaction(int Id_Transfer, CancellationToken cancellationToken)
        {
            var record = await context.Transfers
              .SingleOrDefaultAsync(x => x.Id_Transfer == Id_Transfer, cancellationToken);

            if (record.Execution_Date.CompareTo(DateTime.Now) > 0 && record.Status.Equals("in progress"))  //jesli transfer sie jeszcze nie wykonal
            {
                record.Status = "cancelled";
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            else
            {
                return false;
            }

        }


        public async Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account,int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            var count = await context.Transfers
              .CountAsync(x => x.Sender_Bank_Account == sender_Bank_Account || x.Receiver_Bank_Account == sender_Bank_Account);

            var transfers = await context.Transfers.Where(x => x.Sender_Bank_Account == sender_Bank_Account || x.Receiver_Bank_Account == sender_Bank_Account)
                .OrderByDescending(x => x.Execution_Date)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, transfers as IEnumerable<Transfer>);
        }


        public async Task<Tuple<int, IEnumerable<Currency>>> ShowCurrencies(int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            var count = await context.Currencies
               .CountAsync();

            List<Currency> currencies = null;

            for (int i = 0; i < count; i++)
            {
                currencies = await context.Currencies.Where(x => x.Id_Currency > 0)
                    .OrderByDescending(x => x.Id_Currency)
                    .Skip(skipCount)
                    .Take(takeCount)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }


            return Tuple.Create(count, currencies as IEnumerable<Currency>);
        }

        public async Task<Tuple<int, IEnumerable<Transfer>>> ShowAwaitingTransfers(int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            var count = await context.Transfers.Where(x => x.Status.Equals("in progress"))
               .CountAsync();

            
            var transfers = await context.Transfers.Where(x => x.Status.Equals("in progress"))
                 .OrderByDescending(x => x.Execution_Date)
                 .Skip(skipCount)
                 .Take(takeCount)
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);
           
            return Tuple.Create(count, transfers as IEnumerable<Transfer>);
        }

        public async Task<bool> MakeTransfers(CancellationToken cancellationToken)
        {
            var transfers = await context.Transfers.Where(x => x.Execution_Date.CompareTo(DateTime.Now) <= 0 && !x.Status.Equals("cancelled") && !x.Status.Equals("executed"))
              .OrderByDescending(x => x.Execution_Date)
              .AsNoTracking()
              .ToListAsync(cancellationToken);

            foreach (var t in transfers)
            {
                var senderAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == t.Sender_Bank_Account, cancellationToken);

                var receiverAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == t.Receiver_Bank_Account, cancellationToken);

                if (senderAccount.Account_Balance < t.Amount)
                {
                    continue;
                }

                if (senderAccount.Currency == receiverAccount.Currency)  //if currencies of both accounts are the asme
                {
                    var difference = senderAccount.Account_Balance - t.Amount;
                    senderAccount.Account_Balance = difference;

                    var sum = receiverAccount.Account_Balance + t.Amount;
                    receiverAccount.Account_Balance = sum;
                    t.Status = "executed";
                }
                else               //if currencies of accounts are different
                {
                    var currency = await context.Currencies
                    .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

                    var difference = senderAccount.Account_Balance - t.Amount * currency.Exchange_Rate;
                    senderAccount.Account_Balance = difference;

                    var sum = receiverAccount.Account_Balance + t.Amount * currency.Exchange_Rate;
                    receiverAccount.Account_Balance = sum;
                    t.Status = "executed";
                }

            }
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> CreateTransfer(Transfer transfer, CancellationToken cancellationToken)
        {
            var senderAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Sender_Bank_Account, cancellationToken);

            var receiverAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Receiver_Bank_Account, cancellationToken);

            if(senderAccount == null || receiverAccount == null)
            {
                return false;
            }

            context.Transfers.Add(transfer);
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
