using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Patient Patient { get; set; }
        public int RoomNumber { get; set; }
    }
}
