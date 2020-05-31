using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class AdministratorDto : AccountDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime employmentDate { get; set; }

    }
}
