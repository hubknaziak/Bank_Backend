using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankCore.Dtos;
using BankCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BankCore.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly DatabaseContext context;

        public LoanRepository(DatabaseContext context) => this.context = context;

        public async Task<bool> ApplyForLoan(Loan_Application loan_Application, Loan_ApplicationDto loan_ApplicationDto, CancellationToken cancellationToken)
        {
            var record = await context.Accounts
               .SingleOrDefaultAsync(x => x.Login == loan_ApplicationDto.ClientLogin, cancellationToken);

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == record.Id_account, cancellationToken);

            loan_Application.Client = record.Id_account;

            var administrators = context.Administrators.ToArray<Administrator>(); //tablica administratorow
            var size = administrators.Length;               //ilosc kont admin
            var rand = new Random();
            int adminNumber = rand.Next(size);  //number of a random admin
            var admin = administrators[adminNumber]; //get a random admin

            loan_Application.Administrator = admin.Id_Administrator;

            context.Loan_Applications.Add(loan_Application);
            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Tuple<int, IEnumerable<Loan_Application>>> ShowAllLoanApplications(int takeCount, int skipCount, int administrator, CancellationToken cancellationToken)
        {
            var count = await context.Loan_Applications
               .CountAsync(x => x.Administrator == administrator);

            var loanApplications = await context.Loan_Applications.Where(x => x.Administrator == administrator)
                .OrderByDescending(x => x.Decicion_Date)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, loanApplications as IEnumerable<Loan_Application>);
        }

        public async Task<Tuple<int, IEnumerable<Loan_Application>>> ShowLoanApplication(int takeCount, int skipCount, int id_client, CancellationToken cancellationToken)
        {
            var count = await context.Loan_Applications
               .CountAsync(x => x.Client == id_client);

            var loanApplications = await context.Loan_Applications.Where(x => x.Client == id_client)
                .OrderByDescending(x => x.Decicion_Date)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, loanApplications as IEnumerable<Loan_Application>);
        }

        public async Task<Tuple<int, IEnumerable<Loan>>> ShowLoan(int takeCount, int skipCount, int id_client, CancellationToken cancellationToken)
        {
            var count = await context.Loans
               .CountAsync(x => x.Client == id_client);

            var loans = await context.Loans.Where(x => x.Client == id_client)
                .OrderByDescending(x => x.Granting_Date)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, loans as IEnumerable<Loan>);
        }

        public async Task<bool> DiscardLoanApplication(int Id_Loan_Application, CancellationToken cancellationToken)
        {
            var loanApplication = await context.Loan_Applications
              .SingleOrDefaultAsync(x => x.Id_Loan_Application == Id_Loan_Application, cancellationToken);

            loanApplication.Status = "rejected";

            try
            {
                return await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }

        public async Task<bool> ConfirmLoanApplication(Loan loan, LoanDto loanDto, CancellationToken cancellationToken)
        {
            var record = await context.Loan_Applications
                .SingleOrDefaultAsync(x => x.Id_Loan_Application == loanDto.Id_Loan_Application, cancellationToken);

            loan.Client = record.Client;
            loan.Administrator = record.Administrator;
            loan.Bank_Account = record.Bank_Account;
            context.Loans.Add(loan);
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
