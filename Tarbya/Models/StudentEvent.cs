using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarbya.Models
{
    public class StudentEvent
    {
        public int ID { get; set; }

        public int studentID { get; set; }

        public int eventID { get; set; }


        public virtual Student student { get; set; }

        public virtual Event eventt { get; set; }
    }
}