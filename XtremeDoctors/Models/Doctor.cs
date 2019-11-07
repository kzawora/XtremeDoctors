using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Models
{
    public class Doctor
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }
        public string Text { get; set; }
        public Appointment[] Appointments { get; set; }
        public DateTime[][] WorkingHours { get; set; }

        public Doctor(string name, string surname, string spec, string text)
        {
            this.Name = name;
            this.Surname = surname;
            this.Specialization = spec;
            this.Text = text;
        }

    }
}
