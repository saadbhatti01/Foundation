using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Donation")]
    public class Donation
    {
        [Key]
        public int dId { get; set; }

        [ForeignKey("Donation_Type")]
        public int dtId { get; set; }

        [ForeignKey("Person")]
        public int perId { get; set; }
        public string dNumber { get; set; }
        public double dAmount { get; set; }
        public string dType { get; set; }
        public string dStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public virtual Person Person { get; set; }
        public virtual Donation_Type  Donation_Type { get; set; }
    }
}
