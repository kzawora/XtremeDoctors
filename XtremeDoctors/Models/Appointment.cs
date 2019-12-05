using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace XtremeDoctors.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int StartSlot { get; set; }
        public int EndSlot { get; set; }
        public int RoomNumber { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        
        public int DoctorId { get; set; }

        [JsonIgnore]
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        [JsonIgnore]
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
