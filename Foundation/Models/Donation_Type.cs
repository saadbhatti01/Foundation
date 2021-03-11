using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Donation_Type")]
    public class Donation_Type
    {
        [Key]
        public int dtId { get; set; }
        public string dtName { get; set; }
        public string dtCode { get; set; }
        public string dtStatus { get; set; }
    }
}
