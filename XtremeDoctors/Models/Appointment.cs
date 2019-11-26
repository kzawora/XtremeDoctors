using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtremeDoctors.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int StartSlot { get; set; }
        public int EndSlot { get; set; }
        public int RoomNumber { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
