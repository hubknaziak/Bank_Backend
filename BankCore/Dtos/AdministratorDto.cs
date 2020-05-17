﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class AdministratorDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Employment_Date { get; set; }

    }
}
