using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XtremeDoctors.Models;
using XtremeDoctors.Data;
using XtremeDoctors.Helpers;

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
            if (patient.Appointments == null)
            {
                return new List<Appointment>();
            }

            return patient.Appointments;
        }

        public Appointment MakeAppointment(int doctorId, DateTime date, string hour)
        {
            Patient patient = database.Patients.Find(1); //TEMP - todo get from context
            Doctor doctor = database.Doctors.Find(doctorId);

            int slot = SlotHelper.HourToSlot(hour);

            Appointment appointment = new Appointment();
            appointment.Doctor = doctor;
            appointment.Patient = patient;
            appointment.Date = date;
            appointment.StartSlot = slot;
            appointment.EndSlot = slot;
            appointment.RoomNumber = 69;

            database.Appointments.Add(appointment);
            database.SaveChanges();

            return appointment;
        }
    }
}
