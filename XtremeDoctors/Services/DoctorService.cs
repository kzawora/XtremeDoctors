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

        public string[] GetAvailableDays(Doctor doctor, DateTime startingDay, int daysForward = 14)
        {
            List<string> availableDays = new List<string>();
            DateTime[] possibleDays = new DateTime[daysForward];
            for (int i = 0; i < daysForward; i++)
            {
                possibleDays[i] = startingDay.Date.AddDays(i);
            }
            foreach (DateTime possibleDay in possibleDays)
            {
                string[] freeSlots = ComputeFreeSlots(doctor, possibleDay);
                if (freeSlots.Length != 0)
                {
                    availableDays.Add(possibleDay.ToString("yyyy-MM-dd"));
                }
            }
            return availableDays.ToArray();
        }

        public WorkingHours getWorkingHoursForDay(Doctor doctor, DateTime date)
        {
            return database.WorkingHours
                .Where(w => w.Doctor.Id == doctor.Id)
                .Where(w => w.Date.DayOfWeek == date.DayOfWeek)
                .OrderByDescending(w => w.Date.Ticks)
                .FirstOrDefault();
        }

        public Appointment[] getAppointmentsForDay(Doctor doctor, DateTime date)
        {
            return database.Appointments
                .Where(a => a.Doctor.Id == doctor.Id)
                .Where(a => a.Date.Date == date.Date)
                .ToArray();
        }

        public string[] ComputeFreeSlots(Doctor doctor, DateTime date)
        {
            WorkingHours workingHours = getWorkingHoursForDay(doctor, date);

            if (workingHours == null)
            {
                return new string[0];
            }

            Appointment[] appointments = getAppointmentsForDay(doctor, date);
            List<int> freeSlots = new List<int>();

            for (int slot = workingHours.StartSlot; slot <= workingHours.EndSlot; slot++)
            {
                bool SlotTaken = appointments
                    .Where(a => a.StartSlot <= slot && slot <= a.EndSlot)
                    .Any();
                if (SlotTaken == false)
                {
                    freeSlots.Add(slot);
                }
            }

            if (freeSlots.Count == 0)
            {
                return new string[0];
            }

            List<string> freeHours = new List<string>();

            foreach (int Slot in freeSlots)
            {
                freeHours.Add(SlotHelper.SlotToHour(Slot));
            }

            return freeHours.ToArray();
        }

    }
}
