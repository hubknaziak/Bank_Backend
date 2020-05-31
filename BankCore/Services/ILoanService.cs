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

        Task<bool> ConfirmLoanApplication(AdminLoanApplicationDto adminLoanApplicationDto, CancellationToken cancellationToken);   //update

        Task<bool> ApplyForLoan(Loan_ApplicationDto Loan_ApplicationDto, CancellationToken cancellationToken); //de facto create

        Task<Tuple<int, IEnumerable<Loan_Application>>> ShowAllLoanApplications(int takeCount, int skipCount, int administrator, CancellationToken cancellationToken);

        Task<IEnumerable<Loan_ApplicationDto>> ShowLoanApplication(string login, CancellationToken cancellationToken);

        Task<IEnumerable<AdminLoanApplicationDto>> GetAdminLoanApplications(string login, CancellationToken cancellationToken);

        Task<IEnumerable<LoanDto>> ShowLoan(string login, CancellationToken cancellationToken);
    }


}
