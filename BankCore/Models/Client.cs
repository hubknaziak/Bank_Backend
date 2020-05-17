using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Models
{
    public class Client
    {
        [Key]
        [ForeignKey("Id_Account")]
        [InverseProperty("Clients")]
        public int Id_Client { get; set; }

        [Required]
        [StringLength(12)]
        public string Phone_Number { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }


    }
}
