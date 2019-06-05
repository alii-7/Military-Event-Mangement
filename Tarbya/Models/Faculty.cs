using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarbya.Models
{
    public class Faculty
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string facultyName { get; set; } 
        
    }
}