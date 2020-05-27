using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class Bank_AccountDto
    {
        [Required]
        public int Currency { get; set; }
        [Required]
        public string ClientLogin { get; set; }
    }
}
