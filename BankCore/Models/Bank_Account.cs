using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Models
{
    public class Bank_Account
    {
        [Key]
        public int Id_Bank_Account { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Opening_Date { get; set; }

        [Required]
        public decimal Account_Balance { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        [Required]
        [ForeignKey("Id_Currency")]
        [InverseProperty("Bank_Accounts")]
        public int  Currency { get; set; }

        [Required]
        [ForeignKey("Id_Client")]
        [InverseProperty("Bank_Accounts")]
        public int Client { get; set; }

    }
}
