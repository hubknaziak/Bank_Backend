using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace BankCore.Models
{
    public class Loan_Application
    {
        [Key]
        public int Id_Loan_Application { get; set; }

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
    }
}
