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

        public async Task<bool> CancelTransaction(int Id_Transfer, CancellationToken cancellationToken)
        {
            return await repository.CancelTransaction(Id_Transfer, cancellationToken);
        }


        public async Task<Tuple<int, IEnumerable<Transfer>>> CheckTransactionHistory(int sender_Bank_Account, int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            return await repository.CheckTransactionHistory(sender_Bank_Account, takeCount, skipCount, cancellationToken);
        }


        public async Task<Tuple<int, IEnumerable<Currency>>> ShowCurrencies(int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            return await repository.ShowCurrencies(takeCount, skipCount, cancellationToken);
        }

        public async Task<Tuple<int, IEnumerable<Transfer>>> ShowAwaitingTransfers(int takeCount, int skipCount, CancellationToken cancellationToken)
        {
            return await repository.ShowAwaitingTransfers(takeCount, skipCount, cancellationToken);
        }


        public async Task<bool> MakeTransfers(CancellationToken cancellationToken)
        {
            return await repository.MakeTransfers(cancellationToken);
        }


        public async Task<bool> CreateTransfer(TransferDto transferDto, CancellationToken cancellationToken)
        {
            string stat = "in progress";
            if (transferDto.Excecution_Date.CompareTo(DateTime.Now) <= 0)
            {
                stat = "executed";
            }
            return await repository.CreateTransfer(new Transfer
            {
                Sending_Date = DateTime.Now,
                Execution_Date = transferDto.Excecution_Date,
                Title = transferDto.Title,
                Receiver = transferDto.Receiver,
                Description = transferDto.Description,
                Status = stat,
                Sender_Bank_Account = transferDto.Sender_Bank_Account,
                Receiver_Bank_Account = transferDto.Receiver_Bank_Account
            }, cancellationToken);
        }
    }
}
