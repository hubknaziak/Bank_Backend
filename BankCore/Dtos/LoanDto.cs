using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class LoanDto
    {

        public decimal totalAmount { get; set; }

        public decimal outstandingAmount { get; set; }

        public decimal rateOfInterest { get; set; }

        public decimal installmentsCount { get; set; } //DECIMAL

        public decimal installment { get; set; }

        public int bankAccountId { get; set; }

        public string status { get; set; }

        [DataType(DataType.Date)]
        public DateTime endOfRepayment { get; set; }

    }
}
