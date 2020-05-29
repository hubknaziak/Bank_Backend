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
        public string Login { get; set; }
        //[Required]
        public string Password { get; set; }
        public string NewPassword { get; set; }
        //[Required]
        public string First_name { get; set; }
        public string NewFirst_name { get; set; }
        //[Required]
        public string Last_name { get; set; }
        public string NewLastName { get; set; }

    }
}
