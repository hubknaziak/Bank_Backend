using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class AdminLoanApplicationDto
    {
        public int loanApplicationId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime submissionDate { get; set; }
        public decimal amount { get; set; }
        public decimal installmentsCount { get; set; }
        public int repaymentTime { get; set; }
        public int bankAccountId { get; set; }
        public string status { get; set; }
    }
}
