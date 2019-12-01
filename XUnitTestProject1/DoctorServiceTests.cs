using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using XtremeDoctors.Data;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using Xunit;

namespace XtremeDoctorsUnitTests
{
    public class DoctorServiceUnitTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            ApplicationDbContext databaseContext = new ApplicationDbContext(options);

            return databaseContext;
        }

        [Fact]
        public void ComputeFreeSlots_Should_Return_Not_Empty_Array_Of_Hours_When_Given_Working_Hours_And_Appointment()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);
            Doctor doctor = new Doctor
            {
                Id = 1,
                Name = "Dr",
                Surname = "Etker",
                Specialization = "Kisiele",
                Text = "Very good doctor"
            };
            // 3:45-4:30 appointment
            Appointment appointment = new Appointment
            {
                Id = 1,
                Date = new DateTime(2019, 11, 26),
                StartSlot = 15,
                EndSlot = 17,
                Patient = null,
                Doctor = doctor,
                RoomNumber = 3,
            };
            // 2:30-12:30 working hours
            WorkingHours workingHours = new WorkingHours
            {
                Id = 1,
                Date = new DateTime(2019, 11, 26),
                StartSlot = 10,
                EndSlot = 50,
                Doctor = doctor,
            };
            dbContext.Doctors.Add(doctor);
            dbContext.Appointments.Add(appointment);
            dbContext.WorkingHours.Add(workingHours);
            dbContext.SaveChanges();

            // Act
            string[] freeSlots = doctorService.ComputeFreeSlots(doctor, new DateTime(2019, 11, 26));

            // Assert
            Assert.NotNull(freeSlots);
            Assert.NotEmpty(freeSlots);
            string[] invalidSlots = new string[] { "3:45", "4:00", "4:15" };
            bool contains = invalidSlots.Any(c => freeSlots.Contains(c));
            Assert.False(contains);
        }


        [Fact]
        public void GetAvailableDays_Should_Return_Two_Days_When_One_Day_Is_Full()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);
            Doctor doctor = new Doctor
            {
                Id = 1,
                Name = "Dr",
                Surname = "Etker",
                Specialization = "Kisiele",
                Text = "Very good doctor"
            };
            List<Appointment> appointments = new List<Appointment>
            {

                // 3:45-4:30 appointment on 2019-11-26
                new Appointment
                {
                    Id = 1,
                    Date = new DateTime(2019, 11, 26),
                    StartSlot = 15,
                    EndSlot = 17,
                    Patient = null,
                    Doctor = doctor,
                    RoomNumber = 3,
                },
                // 2:30-3:00 appointment on 2019-11-27
                new Appointment
                {
                    Id = 2,
                    Date = new DateTime(2019, 11, 27),
                    StartSlot = 10,
                    EndSlot = 11,
                    Patient = null,
                    Doctor = doctor,
                    RoomNumber = 3,
                },
                // 3:00-3:30 appointment on 2019-11-27
                new Appointment
                {
                    Id = 3,
                    Date = new DateTime(2019, 11, 27),
                    StartSlot = 12,
                    EndSlot = 13,
                    Patient = null,
                    Doctor = doctor,
                    RoomNumber = 3,
                },
                new Appointment
                {
                    Id = 4,
                    Date = new DateTime(2019, 11, 28),
                    StartSlot = 15,
                    EndSlot = 17,
                    Patient = null,
                    Doctor = doctor,
                    RoomNumber = 3,
                }
            };

            List<WorkingHours> workingHours = new List<WorkingHours>
            {
                // 2019-11-26, 2:30-12:30 working hours, single appointment
                new WorkingHours
                {
                    Id = 1,
                    Date = new DateTime(2019, 11, 26),
                    StartSlot = 10,
                    EndSlot = 50,
                    Doctor = doctor,
                },
                // 2019-11-27, 2:30-3:30 working hours, appointments full
                new WorkingHours
                {
                    Id = 2,
                    Date = new DateTime(2019, 11, 27),
                    StartSlot = 10,
                    EndSlot = 13,
                    Doctor = doctor,
                },
                // 2019-11-28, 2:30-3:30 working hours, no appointments
                new WorkingHours
                {
                    Id = 3,
                    Date = new DateTime(2019, 11, 28),
                    StartSlot = 10,
                    EndSlot = 50,
                    Doctor = doctor,
                }
            };

            dbContext.Doctors.Add(doctor);
            foreach (Appointment appointment in appointments)
            {
                dbContext.Appointments.Add(appointment);
            }
            foreach (WorkingHours workingHoursElem in workingHours)
            {
                dbContext.WorkingHours.Add(workingHoursElem);
            }
            dbContext.SaveChanges();

            // Act
            string[] availableDays = doctorService.GetAvailableDays(doctor, new DateTime(2019, 11, 26), 14);

            // Assert
            Assert.NotNull(availableDays);
            string[] expectedDays = new string[] { "2019-11-26", "2019-11-28", "03-12-2019", "05-12-2019" };
            Assert.Collection(availableDays,
                i => Assert.Equal("2019-11-26", i),
                i => Assert.Equal("2019-11-28", i),
                i => Assert.Equal("2019-12-03", i),
                i => Assert.Equal("2019-12-04", i),
                i => Assert.Equal("2019-12-05", i)
            );
        }


        [Fact]
        public void givenWorkingHoursForSpecifiedDayAreSetWhenGetingWorkingHoursForDayThenTheyAreReturned()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);

            Doctor doctor = new Doctor { Id = 1,  };

            List<WorkingHours> workingHours = new List<WorkingHours>
            {
                new WorkingHours
                {
                    Id = 1,
                    Date = new DateTime(2019, 11, 11),
                    StartSlot = 14,
                    EndSlot = 26,
                    Doctor = doctor,
                },
            };

            dbContext.Doctors.Add(doctor);
            foreach (WorkingHours workingHoursElem in workingHours)
            {
                dbContext.WorkingHours.Add(workingHoursElem);
            }
            dbContext.SaveChanges();

            // Act
            WorkingHours sampledWorkingHours = doctorService.GetWorkingHoursForDay(doctor, new DateTime(2019, 11, 11));

            // Assert
            Assert.NotNull(sampledWorkingHours);
            Assert.Equal(14, sampledWorkingHours.StartSlot);
            Assert.Equal(26, sampledWorkingHours.EndSlot);
        }

        [Fact]
        public void givenWorkingHoursForSpecifiedDayAreNotSetWhenGetingWorkingHoursForDayThenTryToTakeFromLastWeekKnown()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);

            Doctor doctor = new Doctor { Id = 1, };

            List<WorkingHours> workingHours = new List<WorkingHours>
            {
                new WorkingHours
                {
                    Id = 1,
                    Date = new DateTime(2019, 06, 03), // earlier monday
                    StartSlot = 13,
                    EndSlot = 14,
                    Doctor = doctor,
                },
                new WorkingHours
                {
                    Id = 2,
                    Date = new DateTime(2019, 06, 10), // later monday
                    StartSlot = 20,
                    EndSlot = 26,
                    Doctor = doctor,
                },
            };

            dbContext.Doctors.Add(doctor);
            foreach (WorkingHours workingHoursElem in workingHours)
            {
                dbContext.WorkingHours.Add(workingHoursElem);
            }
            dbContext.SaveChanges();

            // Act
            WorkingHours mondayWorkingHours = doctorService.GetWorkingHoursForDay(doctor, new DateTime(2019, 11, 11));
            WorkingHours tuesdayWorkingHours = doctorService.GetWorkingHoursForDay(doctor, new DateTime(2019, 11, 12));

            // Assert
            Assert.NotNull(mondayWorkingHours);
            Assert.Equal(20, mondayWorkingHours.StartSlot);
            Assert.Equal(26, mondayWorkingHours.EndSlot);
            Assert.Null(tuesdayWorkingHours);
        }
    }
}
