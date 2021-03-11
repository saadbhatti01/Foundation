using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Location")]
    public class Location
    {
        [Key]
        public int locId { get; set; }
        public string locName { get; set; }
        public string locContactOne { get; set; }
        public string locContactTwo { get; set; }
        public string locFax { get; set; }
        public string PostalCode { get; set; }
        public bool locStatus { get; set; }

        [ForeignKey("Country")]
        public int cId { get; set; }

        [ForeignKey("City")]
        public int ctId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
    }
}
