using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zero_hunger.DTO
{
    public class assignEmpDto
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int emp_id { get; set; }
    }
}