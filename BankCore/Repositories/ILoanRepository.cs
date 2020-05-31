using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;

namespace BankCore.Repositories
{
    public interface ILoanRepository
    {
        Task<bool> DiscardLoanApplication(int Id_Loan_Application, CancellationToken cancellationToken); //DONE

        Task<bool> ConfirmLoanApplication(AdminLoanApplicationDto adminLoanApplicationDto, CancellationToken cancellationToken); //DONE

        Task<bool> ApplyForLoan(Loan_Application loan_Application, Loan_ApplicationDto loan_ApplicationDto, CancellationToken cancellationToken);   //DONE

        Task<Tuple<int, IEnumerable<Loan_Application>>> ShowAllLoanApplications(int takeCount, int skipCount, int administrator, CancellationToken cancellationToken);   //DONE

        Task< IEnumerable<Loan_ApplicationDto>> ShowLoanApplication(string login, CancellationToken cancellationToken);   //DONE

        Task<IEnumerable<AdminLoanApplicationDto>> GetAdminLoanApplications(string login, CancellationToken cancellationToken);   //DONE

        Task<IEnumerable<LoanDto>> ShowLoan(string login, CancellationToken cancellationToken);   //DONE
    }
}
