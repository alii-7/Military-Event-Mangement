using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tarbya.ViewModels
{
    public class SearchViewModel
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Search By Name")]
        public string searchString1 { get; set; }


        [Display(Name = "Search By SSN")]
        public string searchString2 { get; set; }


        [Display(Name = "Search By Phone")]
        public string searchString3 { get; set; }
    }
}