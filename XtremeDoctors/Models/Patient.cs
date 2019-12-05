using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace XtremeDoctors.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [InverseProperty("Patient")]
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }

        [InverseProperty("Patient")]
        [JsonIgnore]
        public User User { get; set; }
    }
}
