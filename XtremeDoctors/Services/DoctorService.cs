﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public Doctor AddDoctor(Doctor doctor)
        {
            database.Add(doctor);
            database.SaveChanges();
            return doctor;
        }

        public Doctor EditDoctor(Doctor old, Doctor edited)
        {
            old.Name = edited.Name;
            old.Surname = edited.Surname;
            old.Specialization = edited.Specialization;
            old.Text = edited.Text;
            database.Update(old);
            database.SaveChanges();
            return old;
        }

        public Doctor RemoveDoctor(Doctor doctor)
        {
            database.Remove(doctor);
            database.SaveChanges();
            return doctor;
        }

        public bool IsSlotRangeAvailable(Doctor doctor, DateTime date, int slotBegin, int slotEnd)
        {
            for (int i = slotBegin; i <= slotEnd; i++)
            {
                if (!IsSlotAvailable(doctor, date, i))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSlotAvailable(Doctor doctor, DateTime date, int slot)
        {
            return GetFreeSlotsForDate(doctor, date).Contains(slot);
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
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek dayOfWeek = now.AddDays(i).DayOfWeek;
                result[(int)dayOfWeek] = GetHoursStringForDayOfWeek(doctor, dayOfWeek);
            }
            return result;
        }

        public List<int> GetFreeSlotsForDate(Doctor doctor, DateTime date)
        {
            WorkingHours[] workingHours = GetWorkingHoursForDay(doctor, date);
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
            return freeSlots;
        }

        public List<string> GetFreeHoursForDate(Doctor doctor, DateTime date)
        {
            return GetFreeSlotsForDate(doctor, date).Select(x => SlotHelper.SlotToHour(x)).ToList();
        }

        public bool IsSlotStringValid(string slotString)
        {
            Regex rgx = new Regex("^(([0-1][0-9])|2[0-3]):(00|15|30|45)$");
            return rgx.IsMatch(slotString);
        }

        public string[] ComputeFreeSlots(Doctor doctor, DateTime date)
        {
            WorkingHours[] workingHours = GetWorkingHoursForDay(doctor, date);

            if (workingHours.Length == 0)
            {
                return new string[0];
            }

            List<int> freeSlots = GetFreeSlotsForDate(doctor, date);

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

        public WorkingHours[] GetWorkingHoursForDoctorId(int id)
        {
            Doctor doctor = FindDoctor(id);
            return doctor != null ? GetWorkingHoursForDoctor(doctor) : null;
        }
        public WorkingHours[] GetWorkingHoursForDoctor(Doctor doctor)
        {
            var workingHours = database.WorkingHours
                  .Where(w => w.Doctor.Id == doctor.Id)
                  .OrderBy(w => w.Date.Date)
                  .ToArray();
            if (workingHours is null || workingHours.Length == 0)
            {
                return null;
            }
            return workingHours;
        }

        public WorkingHours AddWorkingHours(WorkingHours workingHours)
        {
            database.WorkingHours.Add(workingHours);
            database.SaveChanges();
            return workingHours;
        }

        public WorkingHours EditWorkingHours(WorkingHours old, WorkingHours edited)
        {
            old.StartSlot = edited.StartSlot;
            old.EndSlot = edited.EndSlot;
            old.Date = edited.Date;
            old.DoctorId = edited.DoctorId;
            database.WorkingHours.Update(old);
            database.SaveChanges();
            return old;
        }
        public WorkingHours RemoveWorkingHours(int workingHoursId)
        {
            WorkingHours workingHours = GetWorkingHoursById(workingHoursId);
            if (workingHours == null) return null;
            database.WorkingHours.Remove(workingHours);
            database.SaveChanges();
            return workingHours;
        }
        public WorkingHours GetWorkingHoursById(int id)
        {
            return database.WorkingHours.Find(id);
        }
    }
}
