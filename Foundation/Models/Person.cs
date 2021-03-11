using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int perId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string code { get; set; }
        public string cnic { get; set; }
        public string email { get; set; }
        public string contactOne { get; set; }
        public string contactTwo { get; set; }
        public string addressOne { get; set; }

        [ForeignKey("Country")]
        public int cId { get; set; }

        [ForeignKey("City")]
        public int ctId { get; set; }
        public string postalCode { get; set; }
        public string perImage { get; set; }

        [ForeignKey("Role")]
        public int roleId { get; set; }
        public string perStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        public virtual Role Role { get; set; }
    }
}
