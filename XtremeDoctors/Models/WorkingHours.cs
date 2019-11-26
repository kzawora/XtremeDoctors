using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Models
{
    public class WorkingHours
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int StartSlot { get; set; }
        public int EndSlot { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
    }
}
