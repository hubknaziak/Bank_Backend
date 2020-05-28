using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Models
{
    public class Transfer
    {
        [Key]
        public int Id_Transfer { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Sending_Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Execution_Date { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Receiver { get; set; }


        [Required]
        [StringLength(100)]
        public string Description { get; set; }


        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        [Required]
        [ForeignKey("Id_Bank_Account")]
        [InverseProperty("Transfers")]
        public int Sender_Bank_Account { get; set; }

        [Required]
        [ForeignKey("Id_Bank_Account")]
        [InverseProperty("Transfers")]
        public int Receiver_Bank_Account { get; set; }

    }
}
