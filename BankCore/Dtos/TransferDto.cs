using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Dtos
{
    public class TransferDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Excecution_Date { get; set; }

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
        public int Sender_Bank_Account { get; set; }

        [Required]
        public int Receiver_Bank_Account { get; set; }
    }
}
