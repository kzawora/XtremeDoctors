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
        private DoctorService doctorService;
        public AppointmentService(ApplicationDbContext database, DoctorService doctorService)
        {
            this.database = database;
            this.doctorService = doctorService;
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

        public Appointment MakeAppointment(int doctorId, int patientId, DateTime date, string hour, string comment)
        {
            Patient patient = database.Patients.Find(patientId);
            Doctor doctor = database.Doctors.Find(doctorId);
            
            if (!doctorService.IsSlotStringValid(hour))
                return null;

            int slot = SlotHelper.HourToSlot(hour);
            
            if (!doctorService.IsSlotAvailable(doctor, date, slot)) 
                return null;

            Appointment appointment = new Appointment();
            appointment.Doctor = doctor;
            appointment.Patient = patient;
            appointment.Date = date;
            appointment.StartSlot = slot;
            appointment.EndSlot = slot;
            appointment.Comment = comment;
            appointment.RoomNumber = 69; //TODO !!!!!!

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

        public List<Appointment> GetAllAppointments()
        {
            return database.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToList();
        }

        public Appointment CancelAppointmentById(int appointmentId)
        {
            var toRemove = database.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Id == appointmentId)
                .FirstOrDefault();
            if (toRemove == null)
                return null;
            database.Appointments.Remove(toRemove);
            database.SaveChanges();
            return toRemove;
        }

        internal Appointment UpdateAppointmentComment(int appointmentId, string comment)
        {
            var toEdit = database.Appointments
                 .Include(a => a.Doctor)
                 .Include(a => a.Patient)
                 .Where(a => a.Id == appointmentId)
                 .FirstOrDefault();
            toEdit.Comment = comment;
            database.Appointments.Update(toEdit);
            database.SaveChanges();
            return toEdit;
        }
    }
}
