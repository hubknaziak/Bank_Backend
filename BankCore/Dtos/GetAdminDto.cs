using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class GetAdminDto
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Employment_Date { get; set; }
    }
}
