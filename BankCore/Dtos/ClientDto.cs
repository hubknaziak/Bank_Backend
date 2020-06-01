using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class ClientDto : AccountDto
    {
        [StringLength(12)]
        public string phoneNumber { get; set; }

        [StringLength(50)]
        public string address { get; set; }

    }
}
