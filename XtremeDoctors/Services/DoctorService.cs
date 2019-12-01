﻿using System;
using System.Collections.Generic;
using System.Linq;
using XtremeDoctors.Data;
using XtremeDoctors.Helpers;
using XtremeDoctors.Models;
using System.Linq;

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

        public WorkingHours[] GetWorkingHoursForDay(Doctor doctor, DayOfWeek dayOfWeek)
        {
            var hoursForDayOfWeek = database.WorkingHours
                .Where(w => w.Doctor.Id == doctor.Id)
                .Where(w => w.Date.DayOfWeek == dayOfWeek)
                .GroupBy(w => w.Date.Date)
                .OrderByDescending(group => group.First().Date.Date)
                .ToArray();
            if (hoursForDayOfWeek.Length == 0)
            {
                return new WorkingHours[0];
            }
            return hoursForDayOfWeek.First().ToArray();
        }

        public WorkingHours[] GetWorkingHoursForDay(Doctor doctor, DateTime date)
        {
            return GetWorkingHoursForDay(doctor, date.DayOfWeek);
        }

        public Appointment[] GetAppointmentsForDay(Doctor doctor, DateTime date)
        {
            return database.Appointments
                .Where(a => a.Doctor.Id == doctor.Id)
                .Where(a => a.Date.Date == date.Date)
                .ToArray();
        }

        public string GetHoursStringForDayOfWeek(Doctor doctor, DayOfWeek dayOfWeek)
        {
            WorkingHours[] hours = GetWorkingHoursForDay(doctor, dayOfWeek);
            if (hours == null || hours.Length == 0)
            {
                return "Unavailable";
            }
            return string.Join(", ",
                hours.Select(h => SlotHelper.SlotToHour(h.StartSlot) + " - " + SlotHelper.SlotToHour(h.EndSlot))
            );
        }

        public string[] GetHoursStringForWholeWeek(Doctor doctor)
        {
            string[] result = new string[7];
            DateTime now = DateTime.Now;
            for (int i=0; i < 7; i++)
            {
                DayOfWeek dayOfWeek = now.AddDays(i).DayOfWeek;
                result[(int)dayOfWeek] = GetHoursStringForDayOfWeek(doctor, dayOfWeek);
            }
            return result;
        }

        public string[] ComputeFreeSlots(Doctor doctor, DateTime date)
        {
            WorkingHours[] workingHours = GetWorkingHoursForDay(doctor, date);

            if (workingHours.Length == 0)
            {
                return new string[0];
            }

            Appointment[] appointments = GetAppointmentsForDay(doctor, date);
            List<int> freeSlots = new List<int>();

            foreach (WorkingHours hours in workingHours)
            {
                for (int slot = hours.StartSlot; slot <= hours.EndSlot; slot++)
                {
                    bool SlotTaken = appointments
                        .Where(a => a.StartSlot <= slot && slot <= a.EndSlot)
                        .Any();
                    if (SlotTaken == false)
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