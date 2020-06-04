using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class Loan_ApplicationDto
    {
        [DataType(DataType.Date)]
        public DateTime submissionDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime decicionDate { get; set; }

        [Required]
        public decimal installmentsCount { get; set; } 

        [Required]
        public decimal amount { get; set; }

        public int repaymentTime { get; set; }

        public string status { get; set; }

        [Required]
        public int bankAccountId { get; set; }
    }
}
