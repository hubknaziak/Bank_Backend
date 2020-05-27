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
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository repository;

        public LoanService(ILoanRepository repository)
        {
            this.repository = repository;
            //this.configuration = configuration;
            // this.secretKey = secretKey;
        }


        public async Task<bool> ApplyForLoan(Loan_ApplicationDto loan_ApplicationDto, CancellationToken cancellationToken)
        {
            return await repository.ApplyForLoan(new Loan_Application
            {
                Submission_Date = loan_ApplicationDto.Submission_Date,
                Decicion_Date = loan_ApplicationDto.Decicion_Date,
                Installments_Count = loan_ApplicationDto.Installments_Count,
                Amount = loan_ApplicationDto.Amount,
                Repayment_Time = loan_ApplicationDto.Repayment_Time,
                Status = "awaiting",
                Bank_Account = loan_ApplicationDto.Bank_Account
            }, loan_ApplicationDto, cancellationToken);
        }

        public async Task<Tuple<int, IEnumerable<Loan_Application>>> ShowAllLoanApplications(int takeCount, int skipCount, int administrator, CancellationToken cancellationToken)
        {
            return await repository.ShowAllLoanApplications(takeCount, skipCount, administrator, cancellationToken);
           // var loan_Applications = await repository.ShowLoanApplications(takeCount, skipCount, administrator, cancellationToken);
           // return Tuple.Create(loan_Applications.Item1, loan_Applications.Item2.Select(x => mapper.Map<NoteDto>(x)));
        }

        public async Task<Tuple<int, IEnumerable<Loan_Application>>> ShowLoanApplication(int takeCount, int skipCount, int id_client, CancellationToken cancellationToken)
        {
            return await repository.ShowLoanApplication(takeCount, skipCount, id_client, cancellationToken);
        }

        public async Task<Tuple<int, IEnumerable<Loan>>> ShowLoan(int takeCount, int skipCount, int id_client, CancellationToken cancellationToken)
        {
            return await repository.ShowLoan(takeCount, skipCount, id_client, cancellationToken);
        }

        public async Task<bool> DiscardLoanApplication(int Id_Loan_Application, CancellationToken cancellationToken)
        {
            return await repository.DiscardLoanApplication(Id_Loan_Application, cancellationToken);
        }

        public async Task<bool> ConfirmLoanApplication(LoanDto loanDto, CancellationToken cancellationToken)
        {
            return await repository.ConfirmLoanApplication(new Loan
            {
                Id_Loan = loanDto.Id_Loan_Application,
                Total_Amount = loanDto.Total_Amount,
                Outstanding_Amount = loanDto.Outstanding_Amount,
                Rate_Of_Interest = loanDto.Rate_Of_Interest,
                Installments_Count = loanDto.Installments_Count,
                Installment = loanDto.Installment,
                Granting_Date = DateTime.Now,
                End_Of_Repayment = loanDto.End_Of_Repayment,
                Status = "unpaid"
            }, loanDto, cancellationToken);
        }
    }
}
