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
            var bankAccount = await context.Bank_Accounts
               .SingleOrDefaultAsync(x => x.Id_Bank_Account == loan_ApplicationDto.bankAccountId, cancellationToken);

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == bankAccount.Client, cancellationToken);

            loan_Application.Client = client.Id_Client;

            var administrators = await context.Administrators
                .OrderByDescending(x => x.Id_Administrator)
                .ToArrayAsync(cancellationToken);
            var size = administrators.Length;               //ilosc kont admin
            int[] numberOfApplications = new int[size];                  //tablica na ilosc wnioskow
            for(int i = 0; i < size; i++)
            {
                var applications = await context.Loan_Applications.CountAsync(x => x.Administrator == administrators[i].Id_Administrator);
                numberOfApplications[i] = applications;
            }
            int min = 500000;
            int index = 0;
            for (int i = 0; i < size; i++)
            {
                if(numberOfApplications[i] < min)
                {
                    min = numberOfApplications[i];
                    index = i;
                }
            }
            var admin = administrators[index];

            //var administrators = context.Administrators.ToArray<Administrator>(); //tablica administratorow
            //var size = administrators.Length;               //ilosc kont admin
            //var rand = new Random();
            //int adminNumber = rand.Next(size);  //number of a random admin
            //var admin = administrators[adminNumber]; //get a random admin

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

        public async Task< IEnumerable<Loan_ApplicationDto>> ShowLoanApplication(string login, CancellationToken cancellationToken)
        {
            var account = await context.Accounts
               .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account, cancellationToken);

            //var count = await context.Loan_Applications
             //  .CountAsync(x => x.Client == id_client);

            var loanApplications = await context.Loan_Applications.Where(x => x.Client == client.Id_Client)
                .OrderByDescending(x => x.Submission_Date)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            int size = loanApplications.Length;

            Loan_ApplicationDto[] loan_ApplicationsDto = new Loan_ApplicationDto[size];
            Loan_ApplicationDto loan_ApplicationDto = new Loan_ApplicationDto();

            for(int i = 0; i < size; i++)
            {
                loan_ApplicationDto = new Loan_ApplicationDto();
                loan_ApplicationDto.submissionDate = loanApplications[i].Submission_Date;
                loan_ApplicationDto.decicionDate = loanApplications[i].Decicion_Date;
                loan_ApplicationDto.amount = loanApplications[i].Amount;
                loan_ApplicationDto.installmentsCount = loanApplications[i].Installments_Count;
                loan_ApplicationDto.repaymentTime = loanApplications[i].Repayment_Time;
                loan_ApplicationDto.bankAccountId = loanApplications[i].Bank_Account;
                loan_ApplicationsDto[i] = loan_ApplicationDto;
            }


            return loan_ApplicationsDto as IEnumerable<Loan_ApplicationDto>;
        }

        public async Task<IEnumerable<AdminLoanApplicationDto>> GetAdminLoanApplications(string login, CancellationToken cancellationToken)
        {
            var account = await context.Accounts
               .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            if(account == null)
            {
                return null;
            }

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account, cancellationToken);

            //var count = await context.Loan_Applications
            //  .CountAsync(x => x.Client == id_client);

            var loanApplications = await context.Loan_Applications.Where(x => x.Client == client.Id_Client && x.Status == "awaiting response")
                .OrderByDescending(x => x.Submission_Date)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            int size = loanApplications.Length;

            AdminLoanApplicationDto[] adminLoan_ApplicationsDto = new AdminLoanApplicationDto[size];
            AdminLoanApplicationDto adminLoan_ApplicationDto = new AdminLoanApplicationDto();

            for (int i = 0; i < size; i++)
            {
                adminLoan_ApplicationDto = new AdminLoanApplicationDto();
                adminLoan_ApplicationDto.loanApplicationId = loanApplications[i].Id_Loan_Application;
                adminLoan_ApplicationDto.firstName = account.First_name;
                adminLoan_ApplicationDto.lastName = account.Last_name;
                adminLoan_ApplicationDto.phoneNumber = client.Phone_Number;
                adminLoan_ApplicationDto.submissionDate = loanApplications[i].Submission_Date;
                adminLoan_ApplicationDto.amount = loanApplications[i].Amount;
                adminLoan_ApplicationDto.installmentsCount = loanApplications[i].Installments_Count;
                adminLoan_ApplicationDto.repaymentTime = loanApplications[i].Repayment_Time;
                adminLoan_ApplicationDto.bankAccountId = loanApplications[i].Bank_Account;
                adminLoan_ApplicationsDto[i] = adminLoan_ApplicationDto;
            }


            return adminLoan_ApplicationsDto as IEnumerable<AdminLoanApplicationDto>;
        }

        public async Task< IEnumerable<LoanDto>> ShowLoan(string login, CancellationToken cancellationToken)
        {
            var account = await context.Accounts
              .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            var client = await context.Clients
              .SingleOrDefaultAsync(x => x.Id_Client == account.Id_account, cancellationToken);

            //var count = await context.Loans
            //   .CountAsync(x => x.Client == id_client);

            var loans = await context.Loans.Where(x => x.Client == client.Id_Client)
                .OrderByDescending(x => x.End_Of_Repayment)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            int size = loans.Length;

            LoanDto[] loansDto = new LoanDto[size];
            LoanDto loanDto = new LoanDto();

            for (int i = 0; i < size; i++)
            {
                loanDto = new LoanDto();
                loanDto.endOfRepayment = loans[i].End_Of_Repayment;
                loanDto.totalAmount = loans[i].Total_Amount;
                loanDto.outstandingAmount = loans[i].Outstanding_Amount;
                loanDto.installmentsCount = loans[i].Installments_Count;
                loanDto.installment = loans[i].Installment;
                loanDto.bankAccountId = loans[i].Bank_Account;
                loanDto.status = loans[i].Status;
                loansDto[i] = loanDto;
            }

            return loansDto as IEnumerable<LoanDto>;
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

        public async Task<bool> ConfirmLoanApplication(AdminLoanApplicationDto adminLoanApplicationDto, CancellationToken cancellationToken)
        {
            var loan_Application = await context.Loan_Applications
                .SingleOrDefaultAsync(x => x.Id_Loan_Application == adminLoanApplicationDto.loanApplicationId, cancellationToken);

            if(adminLoanApplicationDto.status == "rejected")
            {
                loan_Application.Status = "rejected";
                try
                {
                    return await context.SaveChangesAsync(cancellationToken) > 0;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }

            loan_Application.Status = "accepted";

            Loan loan = new Loan();

            loan.Client = loan_Application.Client;
            loan.Administrator = loan_Application.Administrator;
            loan.Bank_Account = loan_Application.Bank_Account;
            loan.Id_Loan = loan_Application.Id_Loan_Application;
            loan.Total_Amount = loan_Application.Amount;
            loan.Outstanding_Amount = loan_Application.Amount;
            loan.Rate_Of_Interest = 3;
            loan.Installments_Count = loan_Application.Installments_Count;
            loan.Installment = 1;
            loan.Granting_Date = DateTime.Now;
            loan.End_Of_Repayment = DateTime.Now.AddMonths((int)loan_Application.Repayment_Time);
            loan.Status = "unpaid";
            loan.Client = loan_Application.Client;
            loan.Administrator = loan_Application.Administrator;
            loan.Bank_Account = loan_Application.Bank_Account;

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
