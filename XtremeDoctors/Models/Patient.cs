using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtremeDoctors.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [InverseProperty("Patient")]
        public List<Appointment> Appointments { get; set; }

        [InverseProperty("Patient")]
        public User User { get; set; }
    }
}
