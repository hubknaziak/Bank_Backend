﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class GetAccountDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string Login { get; set; }
        //[Required]
        public string First_name { get; set; }

        public string Last_name { get; set; }
    }
}