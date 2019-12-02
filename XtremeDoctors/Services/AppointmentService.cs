using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XtremeDoctors.Models;
using XtremeDoctors.Data;
using XtremeDoctors.Helpers;
using Microsoft.EntityFrameworkCore;

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

            List<Appointment> appointments = database.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Patient.Id == patientId)
                .ToList();

            return appointments;
        }

        public Appointment MakeAppointment(int doctorId, int patientId, DateTime date, string hour)
        {
            Patient patient = database.Patients.Find(patientId);
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

        public Appointment GetAppointmentById(int appointmentId)
        {
            return database.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Id == appointmentId)
                .FirstOrDefault();
        }

        public void CancelAppointmentById(int appointmentId)
        {
            var toRemove = database.Appointments.Find(appointmentId);
            database.Appointments.Remove(toRemove);
            database.SaveChanges();
        }
    }
}
