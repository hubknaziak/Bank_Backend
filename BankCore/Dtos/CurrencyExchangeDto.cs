using System;
using System.Collections.Generic;
using System.Text;

namespace BankCore.Dtos
{
    public class CurrencyExchangeDto
    {
        public int transferFrom { get; set; }
        public int transferTo { get; set; }
        public decimal amount { get; set; }
    }
}
