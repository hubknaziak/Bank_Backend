using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class Bank_AccountDto
    {
        public int currencyId { get; set; }
    
        public decimal balance { get; set; }

        public int bankAccountId { get; set; }

        public string currencyName { get; set; }
        
        public string status { get; set; }

    }
}
