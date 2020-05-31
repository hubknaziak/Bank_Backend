using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class GetClientDto 
    {
        public string login { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        [Required]
        public string status { get; set; }

        [Required]
        [StringLength(12)]
        public string phoneNumber { get; set; }

        [StringLength(50)]
        public string address { get; set; }

    }
}
