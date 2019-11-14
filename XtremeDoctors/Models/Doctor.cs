using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtremeDoctors.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }
        public string Text { get; set; }

        [InverseProperty("Doctor")]
        public List<Appointment> Appointments { get; set; }
    }
}
