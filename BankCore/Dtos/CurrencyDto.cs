using System;
using System.Collections.Generic;
using System.Text;

namespace BankCore.Dtos
{
    public class CurrencyDto
    {
        public int currencyId { get; set; }

        public string currencyName { get; set; }

        public decimal exchangeRate { get; set; }
    }
}
