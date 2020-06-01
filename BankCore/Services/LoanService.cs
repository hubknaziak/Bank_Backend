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
        }


        public async Task<bool> ApplyForLoan(Loan_ApplicationDto loan_ApplicationDto, CancellationToken cancellationToken)
        {
            return await repository.ApplyForLoan(new Loan_Application
            {
                Submission_Date = DateTime.Now,
                Installments_Count = loan_ApplicationDto.installmentsCount,
                Amount = loan_ApplicationDto.amount,
                Repayment_Time = loan_ApplicationDto.repaymentTime,
                Status = "awaiting response",
                Bank_Account = loan_ApplicationDto.bankAccountId
            }, loan_ApplicationDto, cancellationToken);
        }

        public async Task<Tuple<int, IEnumerable<Loan_Application>>> ShowAllLoanApplications(int takeCount, int skipCount, int administrator, CancellationToken cancellationToken)
        {
            return await repository.ShowAllLoanApplications(takeCount, skipCount, administrator, cancellationToken);
        }

        public async Task< IEnumerable<Loan_ApplicationDto>> ShowLoanApplication(string login, CancellationToken cancellationToken)
        {
            return await repository.ShowLoanApplication(login, cancellationToken);
        }

        public async Task<IEnumerable<AdminLoanApplicationDto>> GetAdminLoanApplications(string login, CancellationToken cancellationToken)
        {
            return await repository.GetAdminLoanApplications(login, cancellationToken);
        }

        public async Task< IEnumerable<LoanDto>> ShowLoan(string login, CancellationToken cancellationToken)
        {
            return await repository.ShowLoan(login, cancellationToken);
        }

        public async Task<bool> DiscardLoanApplication(int Id_Loan_Application, CancellationToken cancellationToken)
        {
            return await repository.DiscardLoanApplication(Id_Loan_Application, cancellationToken);
        }

       public async Task<bool> ConfirmLoanApplication(AdminLoanApplicationDto adminLoanApplicationDto, CancellationToken cancellationToken)
        {
            return await repository.ConfirmLoanApplication(adminLoanApplicationDto, cancellationToken);
        }
    }
}
