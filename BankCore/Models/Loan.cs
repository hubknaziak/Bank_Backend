using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Models
{
    public class Loan
    {
        [Key]
        public int Id_Loan { get; set; }

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
        public DateTime Granting_Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime End_Of_Repayment { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        [Required]
        [ForeignKey("Id_Client")]
        [InverseProperty("Loan_Applications")]
        public int Client { get; set; }

        [Required]
        [ForeignKey("Id_Administator")]
        [InverseProperty("Loan_Applications")]
        public int Administrator { get; set; }

        [Required]
        [ForeignKey("Id_Bank_Account")]
        [InverseProperty("Loan_Applications")]
        public int Bank_Account { get; set; }



        public virtual Administrator AdministratorNavigation { get; set; }

        public virtual Bank_Account BankAccountNavigation { get; set; }

        public virtual Client ClientNavigation { get; set; }

    }
}
