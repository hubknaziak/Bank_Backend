using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankCore.Dtos
{
    public class ClientDto
    {
        //[Required]
        [StringLength(12)]
        public string Phone_Number { get; set; }

        [StringLength(12)]
        public string NewPhone_Number { get; set; }

        [StringLength(50)]
        public string Address { get; set; }
        
        [StringLength(12)]
        public string NewAddress { get; set; }

    }
}
