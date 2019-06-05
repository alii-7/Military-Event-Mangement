using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarbya.Models
{
    public class EducationalQualification
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string educationalQualificationName { get; set; }

    }
}