using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarbya.Models;

namespace Tarbya.ViewModels
{
    public class CreateStudentViewModel
    {
        public IEnumerable<Year> years { get; set; }

        public IEnumerable<Faculty> faculties { get; set; }

        public IEnumerable<EducationalQualification> educationalQualifications { get; set; }

        public Student student { get; set; }
    }
}