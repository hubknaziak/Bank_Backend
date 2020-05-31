using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class GetAdminDto
    {
        public string login { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        [Required]
        public string status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime employmentDate { get; set; }
    }
}
