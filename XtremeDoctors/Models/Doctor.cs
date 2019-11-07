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

        public Doctor(string name, string surname, string spec)
        {
            this.Name = name;
            this.Surname = surname;
            this.Specialization = spec;
        }

    }
}
