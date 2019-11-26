using Microsoft.EntityFrameworkCore;
using System;
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
        public void Should_Return_Not_Empty_Array_Of_Hours_When_Given_Working_Hours_And_Appointment()
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
            Appointment appointment = new Appointment
            {
                Id = 1,
                Day = 1,
                Week = 2,
                Year = 2019,
                Date = new DateTime(2019, 11, 26),
                StartSlot = 15,
                EndSlot = 17,
                Patient = null,
                Doctor = null,
                RoomNumber = 3,
            };
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
        }
    }
}
