using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarbya.Models
{
    public class Event
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string eventName { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Date)]
        public DateTime dateOFEvent { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string description { get; set; }
        
    }
}