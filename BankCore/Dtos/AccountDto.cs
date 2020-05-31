using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class AccountDto
    {
        //[Required]
        [DataType(DataType.Text)]
        public string login { get; set; }
        //[Required]
        public string password { get; set; }
    
        public string firstName { get; set; }
    
        public string lastName { get; set; }

    }
}
