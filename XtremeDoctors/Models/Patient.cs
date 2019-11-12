﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public Patient(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }

    }
}
