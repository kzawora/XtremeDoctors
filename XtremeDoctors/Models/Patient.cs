using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Models
{
    public class Patient
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Patient(string name, string surname, string spec)
        {
            this.Name = name;
            this.Surname = surname;
        }

    }
}
