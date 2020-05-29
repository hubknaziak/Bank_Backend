using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class LoanDto
    {
        [Required]
        public int Id_Loan_Application { get; set; }

        [Required]
        public decimal Total_Amount { get; set; }

        [Required]
        public decimal Outstanding_Amount { get; set; }

        [Required]
        public decimal Rate_Of_Interest { get; set; }

        [Required]
        public decimal Installments_Count { get; set; } //DECIMAL

        [Required]
        public decimal Installment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime End_Of_Repayment { get; set; }

    }
}
