using System;
using System.Linq;
using XtremeDoctors.Data;
using XtremeDoctors.Helpers;
using XtremeDoctors.Models;

namespace XtremeDoctors.Services
{
    public class DoctorService
    {

        private ApplicationDbContext database;
        public DoctorService(ApplicationDbContext database)
        {
            this.database = database;
        }

        public Doctor[] FindAllDoctors()
        {
            return database.Doctors.ToArray();
        }

        public Doctor FindDoctor(int id)
        {
            return database.Doctors.Find(id);
        }

        public string[] GetAvailableDays(int doctorId)
        {
            Doctor doctor = FindDoctor(doctorId);
            WorkingHours[] workingHours = database.WorkingHours.Where(d => d.Doctor.Id == doctor.Id).ToArray();
            Appointment[] appointments = database.Appointments.Where(d => d.Doctor.Id == doctor.Id).ToArray();
            return null;
        }
        public string[] ComputeFreeSlots(int doctorId)
        {

            Doctor doctor = FindDoctor(doctorId);

            WorkingHours[] workingHours = database.WorkingHours.Where(d => d.Doctor.Id == doctor.Id).ToArray();

            Appointment[] appointments = database.Appointments.Where(d => d.Doctor.Id == doctor.Id).ToArray();



            string[] freeSlots = new string[16];

            for (int i = 0; i < 16; i++)
            {
                freeSlots[i] = SlotHelper.SlotToHour(i + 5);
            }

            return freeSlots;
        }


    }
}
