using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class TransferRequestDto
    {
        public string login { get; set; }

        [DataType(DataType.Date)]
        public DateTime sendingDate { get; set; }
    }
}
