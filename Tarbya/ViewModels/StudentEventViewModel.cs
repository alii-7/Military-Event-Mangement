using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tarbya.Models;

namespace Tarbya.ViewModels
{
    public class StudentEventViewModel
    {
        public int ID { get; set; }

        public int eventID { get; set; }

        public Event eventt { get; set; }

        [Required(ErrorMessage = "مطلوب اضافة الطلاب")]
        public List<int?> students { get; set; }
    }
}