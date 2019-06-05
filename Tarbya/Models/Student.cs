using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tarbya.Models
{
    public class Student
    {
        public int ID { get; set; }
        
        [Required (ErrorMessage="مطلوب")]
        [DataType(DataType.Text)]
        public string firstName { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string secondName { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string thirdName { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string fourthName { get; set; }
        
        public string image { get; set; }

        [NotMapped]
        public HttpPostedFileBase imageFile { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "برجاء ادخال ۱٤ رقم")]
        public string socialSecurityNumber { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string gender { get; set; }
        
        [Required(ErrorMessage = "مطلوب")]
        public string religion { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string Blocked { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public DateTime dateOfBirth { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "برجاء ادخال ۱۱ رقم")]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Text)]
        public string address { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int facultyID { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int yearID { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int educationalQualificationID { get; set; }

        public Faculty faculty { get; set; }

        public Year year { get; set; }

        public EducationalQualification educationalQualification { get; set; }

    }
}