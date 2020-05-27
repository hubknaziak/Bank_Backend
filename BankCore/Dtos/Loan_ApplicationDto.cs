using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class Loan_ApplicationDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Submission_Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Decicion_Date { get; set; }

        [Required]
        public decimal Installments_Count { get; set; } //DECIMAL

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Repayment_Time { get; set; }

        [Required]
        public string ClientLogin { get; set; }

        [Required]
        public int Bank_Account { get; set; }
    }
}
