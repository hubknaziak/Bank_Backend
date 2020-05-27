using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Services
{
    public interface ILoanService
    {
        Task<bool> DiscardLoanApplication(int Id_Loan_Application, CancellationToken cancellationToken);   //update

        Task<bool> ConfirmLoanApplication(LoanDto loanDto, CancellationToken cancellationToken);   //update

        Task<bool> ApplyForLoan(Loan_ApplicationDto Loan_ApplicationDto, CancellationToken cancellationToken); //de facto create

        Task<Tuple<int, IEnumerable<Loan_Application>>> ShowAllLoanApplications(int takeCount, int skipCount, int administrator, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<Loan_Application>>> ShowLoanApplication(int takeCount, int skipCount, int id_client, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<Loan>>> ShowLoan(int takeCount, int skipCount, int id_client, CancellationToken cancellationToken);
    }


}
