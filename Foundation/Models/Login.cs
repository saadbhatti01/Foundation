using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    [Table("Login")]
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public int perId { get; set; }
        public string usrName { get; set; }
        public string usrPassword { get; set; }
        public int roleId { get; set; }
        public string usrStatus { get; set; }
    }
}
