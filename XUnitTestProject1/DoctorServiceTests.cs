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
            Doctor doctor = new Doctor  { Id = 1, };
            Appointment appointment = new Appointment // 3:45-4:30 appointment
            {
                Id = 1,  Date = new DateTime(2019, 11, 26),
                StartSlot = 15, EndSlot = 17,
                Doctor = doctor,
            };
            WorkingHours workingHours = new WorkingHours // 2:30-12:30 working hours
            {
                Id = 1, Date = new DateTime(2019, 11, 26),
                StartSlot = 10, EndSlot = 50,
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
            Doctor doctor = new Doctor { Id = 1, };
            var appointments = new[]
            {
                new Appointment  // 3:45-4:30 appointment on 2019-11-26
                {
                    Id = 1, Date = new DateTime(2019, 11, 26),
                    StartSlot = 15, EndSlot = 17,
                    Doctor = doctor,
                },
                new Appointment   // 2:30-3:00 appointment on 2019-11-27
                {
                    Id = 2, Date = new DateTime(2019, 11, 27),
                    StartSlot = 10, EndSlot = 11,
                    Doctor = doctor,
                },
                new Appointment // 3:00-3:30 appointment on 2019-11-27
                {
                    Id = 3, Date = new DateTime(2019, 11, 27),
                    StartSlot = 12, EndSlot = 13,
                    Doctor = doctor,
                },
                new Appointment // 3:45-4:15 appointment on 2019-11-28
                {
                    Id = 4, Date = new DateTime(2019, 11, 28),
                    StartSlot = 15, EndSlot = 17,
                    Doctor = doctor,
                }
            };
            var workingHours = new[]
            {
                new WorkingHours  // 2019-11-26, 2:30-12:30 working hours, single appointment
                {
                    Id = 1, Date = new DateTime(2019, 11, 26),
                    StartSlot = 10,EndSlot = 50,
                    Doctor = doctor,
                },
                new WorkingHours  // 2019-11-27, 2:30-3:30 working hours, appointments full
                {
                    Id = 2, Date = new DateTime(2019, 11, 27),
                    StartSlot = 10, EndSlot = 13,
                    Doctor = doctor,
                },
                new WorkingHours  // 2019-11-28, 2:30-3:30 working hours, no appointments
                {
                    Id = 3, Date = new DateTime(2019, 11, 28),
                    StartSlot = 10, EndSlot = 50,
                    Doctor = doctor,
                }
            };
            dbContext.Doctors.Add(doctor);
            dbContext.Appointments.AddRange(appointments);
            dbContext.WorkingHours.AddRange(workingHours);
            dbContext.SaveChanges();

            // Act
            string[] availableDays = doctorService.GetAvailableDays(doctor, new DateTime(2019, 11, 26), 14);

            // Assert
            Assert.NotNull(availableDays);
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

            Doctor doctor = new Doctor { Id = 1, };
            WorkingHours workingHours = new WorkingHours
            {
                Id = 1, Date = new DateTime(2019, 11, 11),
                StartSlot = 14, EndSlot = 26,
                Doctor = doctor,
            };
            dbContext.Doctors.Add(doctor);
            dbContext.WorkingHours.Add(workingHours);
            dbContext.SaveChanges();

            // Act
            WorkingHours[] sampledWorkingHours = doctorService.GetWorkingHoursForDay(doctor, new DateTime(2019, 11, 11));

            // Assert
            Assert.NotNull(sampledWorkingHours);
            Assert.Single(sampledWorkingHours);
            Assert.Equal(14, sampledWorkingHours[0].StartSlot);
            Assert.Equal(26, sampledWorkingHours[0].EndSlot);
        }

        [Fact]
        public void givenWorkingHoursForSpecifiedDayAreNotSetWhenGetingWorkingHoursForDayThenTryToTakeFromLastWeekKnown()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);

            Doctor doctor = new Doctor { Id = 1, };
            var workingHours = new[]
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
            dbContext.WorkingHours.AddRange(workingHours);
            dbContext.SaveChanges();

            // Act
            WorkingHours[] mondayWorkingHours = doctorService.GetWorkingHoursForDay(doctor, new DateTime(2019, 11, 11));
            WorkingHours[] tuesdayWorkingHours = doctorService.GetWorkingHoursForDay(doctor, new DateTime(2019, 11, 12));

            // Assert
            Assert.NotNull(mondayWorkingHours);
            Assert.Single(mondayWorkingHours);
            Assert.Equal(20, mondayWorkingHours[0].StartSlot);
            Assert.Equal(26, mondayWorkingHours[0].EndSlot);
            Assert.Empty(tuesdayWorkingHours);
        }

        [Fact]
        public void simpleFindersShouldWork()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);
            var doctors = new[]
            {
                new Doctor { Id = 1, Specialization = "a" },
                new Doctor { Id = 2, Specialization = "b" },
                new Doctor { Id = 3, Specialization = "c" },
            };
            dbContext.Doctors.AddRange(doctors);
            dbContext.SaveChanges();

            // Act
            Doctor foundDoctor = doctorService.FindDoctor(2);
            Doctor[] foundDoctors = doctorService.FindAllDoctors();

            // Assert
            Assert.Equal(2, foundDoctor.Id);
            Assert.Equal("b", foundDoctor.Specialization);
            Assert.Collection(foundDoctors,
               doc => Assert.Equal(1, doc.Id),
               doc => Assert.Equal(2, doc.Id),
               doc => Assert.Equal(3, doc.Id)
           );
        }

        [Fact]
        public void givenMutlipleAppointmentsForOneDayOneGettingAppointmentsForThatDayThenReturnThem()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);
            Doctor doctor = new Doctor { Id = 1, };
            var appointments = new[]
            {
                new Appointment  // 3:45-4:30 appointment on 2019-11-20 (week before 27)
                {
                    Id = 1, Date = new DateTime(2019, 11, 20),
                    StartSlot = 15, EndSlot = 17,
                    Doctor = doctor,
                },
                new Appointment   // 2:30-3:00 appointment on 2019-11-27
                {
                    Id = 2, Date = new DateTime(2019, 11, 27),
                    StartSlot = 10, EndSlot = 11,
                    Doctor = doctor,
                },
                new Appointment // 3:00-3:30 appointment on 2019-11-27
                {
                    Id = 3, Date = new DateTime(2019, 11, 27),
                    StartSlot = 12, EndSlot = 13,
                    Doctor = doctor,
                },
                new Appointment // 3:45-4:15 appointment on 2019-11-28
                {
                    Id = 4, Date = new DateTime(2019, 11, 28),
                    StartSlot = 15, EndSlot = 17,
                    Doctor = doctor,
                }
            };
            dbContext.Doctors.Add(doctor);
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();

            // Act
            Appointment[] sampledAppointments = doctorService.GetAppointmentsForDay(doctor, new DateTime(2019, 11, 27));

            // Assert
            Assert.Equal(2, sampledAppointments.Length);
            Assert.Collection(sampledAppointments,
                appointment => Assert.Equal(2, appointment.Id),
                appointment => Assert.Equal(3, appointment.Id)
            );
        }

        [Fact]
        public void givenNoAppointmentsForADayOneGettingAppointmentsForThatDayThenReturnEmptyArray()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);
            Doctor doctor = new Doctor { Id = 1, };
            var appointments = new[]
            {
                new Appointment // 3:45-4:15 appointment on 2019-11-28
                {
                    Id = 4, Date = new DateTime(2019, 11, 28),
                    StartSlot = 15, EndSlot = 17,
                    Doctor = doctor,
                }
            };
            dbContext.Doctors.Add(doctor);
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();

            // Act
            Appointment[] sampledAppointments = doctorService.GetAppointmentsForDay(doctor, new DateTime(2019, 11, 27));

            // Assert
            Assert.Empty(sampledAppointments);
        }

        [Fact]
        public void whenGettingStringsForWorkingHoursOfOneDoctorsThenReturnTheCorrectArray()
        {
            // Arrange
            ApplicationDbContext dbContext = GetDatabaseContext();
            DoctorService doctorService = new DoctorService(dbContext);
            Doctor doctor = new Doctor { Id = 1, };
            var workingHours = new[]
            {
                new WorkingHours // 8:00 - 9:00 on Monday
                {
                    Id = 1, Date = new DateTime(2019, 11, 4),
                    StartSlot = 32, EndSlot = 36,
                    Doctor = doctor,
                },
                new WorkingHours // 8:00 - 10:00 on Tuesday
                {
                    Id = 2, Date = new DateTime(2019, 11, 5),
                    StartSlot = 32, EndSlot = 40,
                    Doctor = doctor,
                },
                new WorkingHours // 8:00 - 11:00 on Wednesday (set from last week)
                {
                    Id = 3, Date = new DateTime(2019, 11, 6),
                    StartSlot = 32, EndSlot = 44,
                    Doctor = doctor,
                },
                new WorkingHours // 8:00 - 12:00 on Thursday (two segments for that day)
                {
                    Id = 4, Date = new DateTime(2019, 11, 7),
                    StartSlot = 32, EndSlot = 48,
                    Doctor = doctor,
                },
                 new WorkingHours // 13:00 - 14:00 on Thursday (two segments for that day)
                {
                    Id = 5, Date = new DateTime(2019, 11, 7),
                    StartSlot = 52, EndSlot = 56,
                    Doctor = doctor,
                },
                new WorkingHours // 8:00 - 13:00 on Friday
                {
                    Id = 6, Date = new DateTime(2019, 11, 8),
                    StartSlot = 32, EndSlot = 52,
                    Doctor = doctor,
                },
                new WorkingHours // 8:00 - 15:00 on Friday (from last week, should be overriden)
                {
                    Id = 7, Date = new DateTime(2019, 11, 9),
                    StartSlot = 32, EndSlot = 56,
                    Doctor = doctor,
                },
            };
            dbContext.Doctors.Add(doctor);
            dbContext.WorkingHours.AddRange(workingHours);
            dbContext.SaveChanges();

            // Act
            string[] sampledHours = doctorService.GetHoursStringForWholeWeek(doctor);

            // Assert
            Assert.Equal(7, sampledHours.Length);
            Assert.Collection(sampledHours,
                hours => hours.Equals("8:00 - 9:00"),
                hours => hours.Equals("8:00 - 10:00"),
                hours => hours.Equals("8:00 - 11:00"),
                hours => hours.Equals("8:00 - 12:00, 13:00 - 14:00"),
                hours => hours.Equals("8:00 - 13:00"),
                hours => hours.Equals("Unavailable"),
                hours => hours.Equals("Unavailable")
            );
        }
    }
}
