using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class CreateAccountDto
    {
        [Required]
        public AccountDto AccountDto { get; set; }

        public ClientDto ClientDto { get; set; }
        public AdministratorDto AdministratorDto { get; set; }
    }
}
