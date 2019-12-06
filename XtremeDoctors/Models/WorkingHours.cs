using Newtonsoft.Json;
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
        public int StartSlot { get; set; }
        public int EndSlot { get; set; }
        public DateTime Date { get; set; }

        public int DoctorId { get; set; }
        [JsonIgnore]
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
    }
}
