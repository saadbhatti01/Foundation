using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("City")]
    public class City
    {
        [Key]
        public int ctId { get; set; }
        public string ctFullName { get; set; }
        public string ctShortName { get; set; }
        public string ctCode { get; set; }
        public bool ctStatus { get; set; }

        [ForeignKey("Country")]
        public int cId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public virtual Country Country { get; set; }
    }
}
