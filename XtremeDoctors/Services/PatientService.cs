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

        public Patient GetPatient(int id)
        {
            return database.Patients.Find(id);
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
        public Patient AddPatient(Patient patient)
        {
            database.Patients.Add(patient);
            database.SaveChanges();
            return patient;
        }

        public Patient RemovePatient(int id)
        {
            Patient p = GetPatient(id);
            if (p == null)
                return null;
            database.Remove(p);
            database.SaveChanges();
            return p;
        }
        public Patient EditPatient(Patient old, Patient edited)
        {
            old.Name = edited.Name;
            old.Surname = edited.Surname;
            database.Update(old);
            database.SaveChanges();
            return old;
        }
    }
}
