using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Tax")]
    public class Tax
    {
        [Key]
        public int taxId { get; set; }
        public string taxType { get; set; }
        public double tax { get; set; }
    }
}
