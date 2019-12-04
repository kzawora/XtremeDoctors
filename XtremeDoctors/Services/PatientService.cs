using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using XtremeDoctors.Data;
using XtremeDoctors.Helpers;
using XtremeDoctors.Models;

namespace XtremeDoctors.Services
{
    public class PatientService
    {

        private ApplicationDbContext database;
        public PatientService(ApplicationDbContext database)
        {
            this.database = database;
        }

        public List<Patient> GetPatients()
        {

            List<Patient> patients = database.Patients
                .Include(a => a.User)
                .ToList();

            return patients;
        }

        public Patient AddPatient(string name, string surname)
        {

            Patient patient = new Patient();
            patient.Name = name;
            patient.Surname = surname;


            database.Patients.Add(patient);
            database.SaveChanges();

            return patient;
        }


    }
}
