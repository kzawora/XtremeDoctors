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
                string[] freeSlots = ComputeFreeSlots(doctor, possibleDay);
                if (freeSlots.Length != 0)
                {
                    availableDays.Add(possibleDay.ToString("YYYY-MM-DD"));
                }
            }
            return availableDays.ToArray();
        }

        public string[] ComputeFreeSlots(Doctor doctor, DateTime date)
        {

            WorkingHours[] allWorkingHours = database.WorkingHours.Where(w => w.Doctor.Id == doctor.Id).Where(w => w.Date.Day == date.Day).ToArray();

            if (allWorkingHours == null)
            {
                return new string[0];
            }

            Appointment[] appointments = database.Appointments.Where(a => a.Doctor.Id == doctor.Id).Where(a => a.Date.Day == date.Day).ToArray();

            List<int> freeSlots = new List<int>();

            foreach (WorkingHours workingHours in allWorkingHours)
            {
                for (int slot = workingHours.StartSlot; slot <= workingHours.EndSlot; slot++)
                {
                    bool SlotAvailable = true;

                    foreach (Appointment appointment in appointments)
                    {
                        if (slot >= appointment.StartSlot && slot <= appointment.EndSlot)
                        {
                            SlotAvailable = false;
                            break;
                        }
                    }

                    if (SlotAvailable)
                    {
                        freeSlots.Add(slot);
                    }
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
