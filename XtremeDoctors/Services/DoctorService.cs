using System;
using System.Collections.Generic;
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

        public string[] GetAvailableDays(Doctor doctor, int daysForward = 14)
        {
            List<string> availableDays = new List<string>();
            DateTime[] possibleDays = new DateTime[daysForward];
            for (int i = 0; i < daysForward; i++)
            {
                possibleDays[i] = DateTime.Now.Date.AddDays(i);
            }
            foreach (DateTime possibleDay in possibleDays)
            {
                string[] freeSlots = ComputeFreeSlots(doctor.Id, possibleDay);
                if (freeSlots.Length != 0)
                {
                    availableDays.Add(possibleDay.ToString("YYYY-MM-DD"));
                }
            }
            return availableDays.ToArray();
        }

        public string[] ComputeFreeSlots(int doctorId, DateTime date)
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
