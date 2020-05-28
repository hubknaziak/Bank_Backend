using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Models
{
    public class Currency
    {
        [Key]
        public int Id_Currency { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        public decimal Exchange_Rate { get; set; }
    }
}
