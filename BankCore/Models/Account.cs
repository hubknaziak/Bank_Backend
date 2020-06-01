using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Models
{
    public class Account
    {
        [Key]
        public int Id_account { get; set; }
        [Required]
        [RegularExpression(@"\d{9}")]
        [StringLength(9)]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set; }


        public virtual Administrator Administrator { get; set; }
        public virtual Client Client { get; set; }
    }
}
