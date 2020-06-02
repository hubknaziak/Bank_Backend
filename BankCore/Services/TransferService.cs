using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;
using BankCore.Repositories;

namespace BankCore.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository repository;

        public TransferService(ITransferRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CancelTransaction(TransferDto transferDto, CancellationToken cancellationToken)
        {
            return await repository.CancelTransaction(transferDto, cancellationToken);
        }


        public async Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account, int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            return await repository.CheckTransactionHistory(sender_Bank_Account, takeCount, skipCount, cancellationToken);
        }


        public async Task< IEnumerable<CurrencyDto>> ShowCurrencies( CancellationToken cancellationToken)
        {
            return await repository.ShowCurrencies( cancellationToken);
        }

        public async Task<object> GetTransfer(int transferId, CancellationToken cancellationToken)
        {
            return await repository.GetTransfer(transferId, cancellationToken);
        }

        public async Task<IEnumerable<TransferDto>> GetTransfers(int bankAccountId, CancellationToken cancellationToken)
        {
            return await repository.GetTransfers(bankAccountId, cancellationToken);
        }

        public async Task<IEnumerable<TransferDto>> GetAdminTransfers(string login,  DateTime sendingDate, CancellationToken cancellationToken)
        {
            return await repository.GetAdminTransfers(login, sendingDate, cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> ShowAwaitingTransfers( CancellationToken cancellationToken)
        {
            return await repository.ShowAwaitingTransfers(cancellationToken);
        }


        public async Task<bool> MakeTransfers(CancellationToken cancellationToken)
        {
            return await repository.MakeTransfers(cancellationToken);
        }


        public async Task<bool> CreateTransfer(TransferDto transferDto, string login, CancellationToken cancellationToken)
        {
            string stat = "in progress";
      
            if(transferDto.executionDate.CompareTo(transferDto.sendingDate) == 0)
            {
                stat = "executed";
            }
            if (transferDto.executionDate.CompareTo(transferDto.sendingDate) < 0)
            {
                return false;
            }

            return await repository.CreateTransfer(new Transfer
            {
                Sending_Date = transferDto.sendingDate,
                Execution_Date = transferDto.executionDate,
                Title = transferDto.title,
                Receiver = transferDto.receiver,
                Description = transferDto.description,
                Status = stat,
                Amount = transferDto.amount,
                Sender_Bank_Account = transferDto.senderBankAccountId,
                Receiver_Bank_Account = transferDto.receiverBankAccountId
            }, cancellationToken) ;
        }

        public async Task<bool> ExchangeMoney(CurrencyExchangeDto currencyExchangeDto, CancellationToken cancellationToken)
        {
            return await repository.ExchangeMoney(new Transfer
            {
                Sending_Date = DateTime.Now,
                Execution_Date = DateTime.Now,
                Title = "Currency exchange",
                Receiver = "Myself",
                Description = "Currency exchange",
                Status = "executed",
                Amount = currencyExchangeDto.amount,
                Sender_Bank_Account = currencyExchangeDto.transferFrom,
                Receiver_Bank_Account = currencyExchangeDto.transferTo
            }, cancellationToken);
        }
    }
}
