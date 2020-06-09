using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace BankCore.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly DatabaseContext context;

        public TransferRepository(DatabaseContext context) => this.context = context;


        public async Task<bool> CancelTransaction(TransferDto transferDto, CancellationToken cancellationToken)
        {
            var transfer = await context.Transfers
              .SingleOrDefaultAsync(x => x.Id_Transfer == transferDto.transferId, cancellationToken);

            if (transfer == null)
            {
                return false;
            }

            var bankAccount = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Sender_Bank_Account, cancellationToken);

            if (transfer.Status.Equals("in progress")) 
            {
                transfer.Status = transferDto.status;
                bankAccount.Account_Balance = bankAccount.Account_Balance + transfer.Amount;
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

            if (count == 0)
            {
                return null;
            }

            var transfers = await context.Transfers.Where(x => x.Sender_Bank_Account == sender_Bank_Account || x.Receiver_Bank_Account == sender_Bank_Account)
                .OrderByDescending(x => x.Execution_Date)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, transfers as IEnumerable<Transfer>);
        }


        public async Task<IEnumerable<CurrencyDto>> ShowCurrencies( CancellationToken cancellationToken)
        {
            var count = await context.Currencies
               .CountAsync();

            if (count == 0)
            {
                return null;
            }

            Currency[] currencies = new Currency[count];
            CurrencyDto [] currenciesDto = new CurrencyDto[count];
            CurrencyDto currencyDto = new CurrencyDto();


            currencies = await context.Currencies.Where(x => x.Id_Currency > 0)
                    .OrderByDescending(x => x.Id_Currency)
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken);
          

            for (int i = 0; i < count; i++)
            {
                currencyDto = new CurrencyDto();
                currencyDto.currencyId = currencies[i].Id_Currency;
                currencyDto.currencyName = currencies[i].Name;
                currencyDto.exchangeRate = currencies[i].Exchange_Rate;
                currenciesDto[i] = currencyDto;
            }


            return currenciesDto as IEnumerable<CurrencyDto>;
        }

        public async Task<object> GetTransfer(int transferId, CancellationToken cancellationToken)
        {
            var transfer = await context.Transfers
               .SingleOrDefaultAsync(x => x.Id_Transfer == transferId, cancellationToken);

            var senderAccount = await context.Bank_Accounts
               .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Sender_Bank_Account, cancellationToken);

            var senderCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

            var receiverAccount = await context.Bank_Accounts
               .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Receiver_Bank_Account, cancellationToken);

            var receiverCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);


            if (transfer == null || transfer.Status == "executed" || transfer.Status == "cancelled")
            {
                return null;
            }

            TransferDto transferDto = new TransferDto();

            transferDto.transferId = transfer.Id_Transfer;
            transferDto.sendingDate = transfer.Sending_Date;
            transferDto.executionDate = transfer.Execution_Date;
            transferDto.title = transfer.Title;
            transferDto.receiver = transfer.Receiver;
            transferDto.description = transfer.Description;
          
                var baseAmount = transfer.Amount * receiverCurrency.Exchange_Rate;
                transferDto.amount = baseAmount / senderCurrency.Exchange_Rate;

            return transferDto;
        }

        public async Task<IEnumerable<TransferDto>> GetTransfers(int bankAccountId, CancellationToken cancellationToken)
        {
            var transfers = await context.Transfers.Where(x => x.Receiver_Bank_Account == bankAccountId || x.Sender_Bank_Account == bankAccountId)
                .OrderByDescending(x => x.Sending_Date)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            Bank_Account[] senderAccounts = new Bank_Account[transfers.Length];
            Bank_Account[] receiverAccounts = new Bank_Account[transfers.Length];
            Currency[] senderCurrencies = new Currency[transfers.Length];
            Currency[] receiverCurrencies = new Currency[transfers.Length];

            for (int i = 0; i < transfers.Length; i++)
            {
                var senderAccount = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfers[i].Sender_Bank_Account, cancellationToken);

                var senderCurrency = await context.Currencies
                  .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

                var receiverAccount = await context.Bank_Accounts
                   .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfers[i].Receiver_Bank_Account, cancellationToken);

                var receiverCurrency = await context.Currencies
                  .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

                senderAccounts[i] = senderAccount;
                receiverAccounts[i] = receiverAccount;
                senderCurrencies[i] = senderCurrency;
                receiverCurrencies[i] = receiverCurrency;
            }

            if (transfers == null)
            {
                return null;
            }

            int size = transfers.Length;

            TransferDto[] transfersDto = new TransferDto[size];
            TransferDto transferDto = new TransferDto();

            for (int i = 0; i < size; i++)
            {
                transferDto = new TransferDto();
                transferDto.transferId = transfers[i].Id_Transfer;
                transferDto.sendingDate = transfers[i].Sending_Date;
                transferDto.executionDate = transfers[i].Execution_Date;
                transferDto.title = transfers[i].Title;
                transferDto.receiver = transfers[i].Receiver;
                transferDto.description = transfers[i].Description;
                // transferDto.amount = transfers[i].Amount;
                var baseAmount = transfers[i].Amount * receiverCurrencies[i].Exchange_Rate;
                transferDto.amount = baseAmount / senderCurrencies[i].Exchange_Rate;

                transferDto.status = transfers[i].Status;
                if (transfers[i].Receiver_Bank_Account == bankAccountId) transferDto.isReceived = true;
                else transferDto.isReceived = false;
                transfersDto[i] = transferDto;
            }

            return transfersDto as IEnumerable<TransferDto>;
        }

        public async Task<IEnumerable<TransferDto>> GetAdminTransfers(string login,  DateTime sendingDate, CancellationToken cancellationToken)
        {
            var account = await context.Accounts
               .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            if(account == null)
            {
                return null;
            }

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account, cancellationToken);

            var bankAccounts = await context.Bank_Accounts.Where(x => x.Client == client.Id_Client)
                .OrderByDescending(x => x.Opening_Date)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            Transfer[] allTransfers = new Transfer[100];

            int index = 0;
            foreach (Bank_Account b in bankAccounts)
            {
                var transfers = await context.Transfers.Where(x => x.Sender_Bank_Account == b.Id_Bank_Account || x.Receiver_Bank_Account == b.Id_Bank_Account)
                    .OrderByDescending(x => x.Sending_Date)
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken);
                for(int i = 0; i < transfers.Length; i++)
                {
                    allTransfers[index] = transfers[i];
                    index++;
                }
            }

            Bank_Account[] senderAccounts = new Bank_Account[index];
            Bank_Account[] receiverAccounts = new Bank_Account[index];
            Currency[] senderCurrencies = new Currency[index];
            Currency[] receiverCurrencies = new Currency[index];

            for (int i = 0; i < index; i++)
            {
                var senderAccount = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == allTransfers[i].Sender_Bank_Account, cancellationToken);

                var senderCurrency = await context.Currencies
                  .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

                var receiverAccount = await context.Bank_Accounts
                   .SingleOrDefaultAsync(x => x.Id_Bank_Account == allTransfers[i].Receiver_Bank_Account, cancellationToken);

                var receiverCurrency = await context.Currencies
                  .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

                senderAccounts[i] = senderAccount;
                receiverAccounts[i] = receiverAccount;
                senderCurrencies[i] = senderCurrency;
                receiverCurrencies[i] = receiverCurrency;
            }


            TransferDto[] transfersDto = new TransferDto[index];
            TransferDto transferDto = new TransferDto();

            index = 0;
            for (int i = 0; i < allTransfers.Length; i++)
            {
                if (allTransfers[i] != null)
                {
                    if (allTransfers[i].Status == "in progress" && allTransfers[i].Sending_Date.Date == sendingDate.Date)
                    {
                        transferDto = new TransferDto();
                        transferDto.transferId = allTransfers[i].Id_Transfer;
                        transferDto.sendingDate = allTransfers[i].Sending_Date;
                        transferDto.executionDate = allTransfers[i].Execution_Date;
                        transferDto.title = allTransfers[i].Title;
                        transferDto.receiver = allTransfers[i].Receiver;
                        transferDto.description = allTransfers[i].Description;
                        //transferDto.amount = allTransfers[i].Amount;
                        var baseAmount = allTransfers[i].Amount * receiverCurrencies[i].Exchange_Rate;
                        transferDto.amount = baseAmount / senderCurrencies[i].Exchange_Rate;

                        transfersDto[index] = transferDto;
                        index++;
                    }
                }
            }
           TransferDto [] tran = new TransferDto[index];
           index = 0;
           for (int i = 0; i < transfersDto.Length; i++)
           {
                if (transfersDto[i] != null)
                {
                    tran[index] = transfersDto[i];
                    index++;
                }
           }

           for(int i = 0; i < tran.Length - 1; i++)
           {
                var temp1 = tran[i];
                for(int j = i + 1; j < tran.Length; j++)
                {
                    var temp2 = tran[j];
                    if(temp1.transferId == temp2.transferId)
                    {
                        tran[i] = null;
                        index--;
                    }
                }
           }

           TransferDto [] result = new TransferDto[index];
           index = 0;
           for (int i = 0; i < tran.Length; i++)
           {
                if (tran[i] != null)
                {
                    result[index] = tran[i];
                    index++;
                }
           }

            return result as IEnumerable<TransferDto>;
        }

        public async Task<IEnumerable<Transfer>> ShowAwaitingTransfers(CancellationToken cancellationToken)
        {
            var transfers = await context.Transfers.Where(x => x.Status.Equals("in progress"))
                 .OrderByDescending(x => x.Execution_Date)
                 .AsNoTracking()
                 .ToArrayAsync(cancellationToken);

            Bank_Account[] senderAccounts = new Bank_Account[transfers.Length];
            Bank_Account[] receiverAccounts = new Bank_Account[transfers.Length];
            Currency[] senderCurrencies = new Currency[transfers.Length];
            Currency[] receiverCurrencies = new Currency[transfers.Length];

            for (int i = 0; i < transfers.Length; i++)
            {
                var senderAccount = await context.Bank_Accounts
              .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfers[i].Sender_Bank_Account, cancellationToken);

                var senderCurrency = await context.Currencies
                  .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

                var receiverAccount = await context.Bank_Accounts
                   .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfers[i].Receiver_Bank_Account, cancellationToken);

                var receiverCurrency = await context.Currencies
                  .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

                senderAccounts[i] = senderAccount;
                receiverAccounts[i] = receiverAccount;
                senderCurrencies[i] = senderCurrency;
                receiverCurrencies[i] = receiverCurrency;

            }

            for(int i = 0; i < transfers.Length; i++)
            {
                var baseAmount = transfers[i].Amount * receiverCurrencies[i].Exchange_Rate;
                transfers[i].Amount = baseAmount / senderCurrencies[i].Exchange_Rate;
            }

            return  transfers as IEnumerable<Transfer>;
        }

        public async Task<bool> MakeTransfers(CancellationToken cancellationToken)
        {
            var transfers = await context.Transfers.Where(x => x.Execution_Date.CompareTo(DateTime.Now) <= 0 && !x.Status.Equals("cancelled") && !x.Status.Equals("executed"))
              .OrderByDescending(x => x.Execution_Date)
              .AsNoTracking()
              .ToListAsync(cancellationToken);

            if (transfers == null)
            {
                return false;
            }

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
                    var senderCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

                    var receiverCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

                    var difference = senderAccount.Account_Balance - t.Amount;
                    senderAccount.Account_Balance = difference;

                    var baseAmount = t.Amount * senderCurrency.Exchange_Rate;
                    receiverAccount.Account_Balance += baseAmount / receiverCurrency.Exchange_Rate;

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

            var senderCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

            var receiverAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Receiver_Bank_Account, cancellationToken);

            var receiverCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

            var record = context.Transfers
             .OrderByDescending(x => x.Id_Transfer).FirstOrDefault();

            if (record == null) { transfer.Id_Transfer = 0; }
            else { transfer.Id_Transfer = record.Id_Transfer + 1; }

            if (senderAccount == null || receiverAccount == null)
            {
                return false;
            }
            else if(senderAccount.Status == "blocked" || senderAccount.Status == "inactive" || senderAccount.Account_Balance < transfer.Amount)
            {
                return false;
            }
            else if (receiverAccount.Status == "blocked" || receiverAccount.Status == "inactive")
            {
                return false;
            }
            senderAccount.Account_Balance = senderAccount.Account_Balance - transfer.Amount;
            
            if(transfer.Status == "in progress")
            {
                context.Transfers.Add(transfer);
                try
                {
                    return await context.SaveChangesAsync(cancellationToken) > 0;
                }
                catch(DbUpdateException)
                {
                    return false;
                }
            }

            if (transfer.Status == "executed")
            {
                if (senderCurrency.Id_Currency == receiverCurrency.Id_Currency)
                {
                    receiverAccount.Account_Balance = receiverAccount.Account_Balance + transfer.Amount;
                }
                else
                {
                    var baseAmount = transfer.Amount * senderCurrency.Exchange_Rate;
                    receiverAccount.Account_Balance += baseAmount / receiverCurrency.Exchange_Rate;
                }
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

        public async Task<bool> ExchangeMoney(Transfer transfer, CancellationToken cancellationToken)
        { 

            var senderAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Sender_Bank_Account, cancellationToken);

            var senderCurrency = await context.Currencies
               .SingleOrDefaultAsync(x => x.Id_Currency == senderAccount.Currency, cancellationToken);

            var receiverAccount = await context.Bank_Accounts
                .SingleOrDefaultAsync(x => x.Id_Bank_Account == transfer.Receiver_Bank_Account, cancellationToken);

            var receiverCurrency = await context.Currencies
              .SingleOrDefaultAsync(x => x.Id_Currency == receiverAccount.Currency, cancellationToken);

            var plnCurrency = await context.Currencies
             .SingleOrDefaultAsync(x => x.Id_Currency == 1, cancellationToken);

            if (senderAccount == null || receiverAccount == null)
            {
                return false;
            }
            else if (senderAccount.Status == "blocked" || senderAccount.Status == "inactive" || senderAccount.Account_Balance < (transfer.Amount * receiverCurrency.Exchange_Rate))
            {
                return false;
            }
            else if (receiverAccount.Status == "blocked" || receiverAccount.Status == "inactive")
            {
                return false;
            }
            senderAccount.Account_Balance = senderAccount.Account_Balance - transfer.Amount;

            //transfer.Amount = transfer.Amount * plnCurrency.Exchange_Rate;
            //receiverAccount.Account_Balance = receiverAccount.Account_Balance + (transfer.Amount * receiverCurrency.Exchange_Rate);

            var baseAmount = transfer.Amount * senderCurrency.Exchange_Rate;
            receiverAccount.Account_Balance += baseAmount / receiverCurrency.Exchange_Rate;

            var transferId = context.Transfers
            .OrderByDescending(x => x.Id_Transfer).FirstOrDefault();

            if (transferId == null) transfer.Id_Transfer = 0;
            else transfer.Id_Transfer = transferId.Id_Transfer + 1;

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
