using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int cId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
        public string cFullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the short name")]
        public string cShortName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Code")]
        public string cCode { get; set; }

        [Required]
        public bool cStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }
}
