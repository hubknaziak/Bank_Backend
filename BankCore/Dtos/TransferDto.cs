using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Dtos
{
    public class TransferDto
    {
        public int transferId { get; set; }

        [DataType(DataType.Date)]
        public DateTime sendingDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime executionDate { get; set; }

        [StringLength(30)]
        public string title { get; set; }
      
        [StringLength(50)]
        public string receiver { get; set; }


        [StringLength(100)]
        public string description { get; set; }


        public decimal amount { get; set; }

        public int senderBankAccountId { get; set; }

        public int receiverBankAccountId { get; set; }

        public bool isReceived { get; set; }

        public string status { get; set; }
    }
}
