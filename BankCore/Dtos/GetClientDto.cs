using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class GetClientDto
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set; }

        [Required]
        [StringLength(12)]
        public string Phone_Number { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

    }
}
