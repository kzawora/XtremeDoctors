using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace XtremeDoctors.Models
{
    public class Doctor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }
        public string Text { get; set; }
        
        [JsonIgnore]
        [InverseProperty("Doctor")]
        public List<Appointment> Appointments { get; set; }
        
        [JsonIgnore]
        [InverseProperty("Doctor")]
        public List<WorkingHours> WorkingHours { get; set; }
    }
}
