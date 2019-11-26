using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XtremeDoctors.Models;
using XtremeDoctors.Data;

namespace XtremeDoctors.Services
{
    public class AppointmentService
    {
        private ApplicationDbContext database;
        public AppointmentService(ApplicationDbContext database)
        {
            this.database = database;
        }

        public List<Appointment> GetAppointmentsForPatient(int patientId)
        {
            Patient patient = database.Patients.Find(patientId);
            return patient.Appointments;
        }
    }
}
