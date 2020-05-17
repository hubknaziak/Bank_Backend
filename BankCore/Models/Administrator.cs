using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankCore.Models
{
    public class Administrator
    {
        [Key]
        [ForeignKey("Id_Account")]
        [InverseProperty("Administrators")]
        public int Id_Administrator { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Employment_Date { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }
    }
}
