using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using XtremeDoctors.Models;
using XtremeDoctors.Helpers;
using Microsoft.AspNetCore.Identity;

namespace XtremeDoctors.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WorkingHours> WorkingHours { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var doctors = new[]
            {
                new
                {
                    Id = 1,
                    Name = "Dr",
                    Surname = "Etker",
                    Specialization = "Kisiele",
                    Text = "Very good doctor"
                },
                new
                {
                    Id = 2,
                    Name = "Dr",
                    Surname = "Pepper",
                    Specialization = "Drinking",
                    Text = "Not so good doctor"
                },
                new
                {
                    Id = 3,
                    Name = "Dr",
                    Surname = "Dre",
                    Specialization = "Wrapping",
                    Text = "Quite good doctor."
                }
            };
            var workingHours = new[]{
                new
                {
                    Id = 1,
                    Date = new DateTime(2019, 11, 11), // Monday
                    StartSlot = SlotHelper.HourToSlot("8:00"),
                    EndSlot = SlotHelper.HourToSlot("16:00"),
                    DoctorId = doctors[0].Id,
                },
                 new
                {
                    Id = 2,
                    Date = new DateTime(2019, 11, 13), // Wednesday
                    StartSlot = SlotHelper.HourToSlot("7:30"),
                    EndSlot = SlotHelper.HourToSlot("15:30"),
                    DoctorId = doctors[0].Id,
                },
                new
                {
                    Id = 3,
                    Date = new DateTime(2019, 11, 13), // Wednesday
                    StartSlot = SlotHelper.HourToSlot("6:30"),
                    EndSlot = SlotHelper.HourToSlot("14:30"),
                    DoctorId = doctors[1].Id,
                }
            };
            modelBuilder.Entity<Doctor>().HasData(doctors);
            modelBuilder.Entity<WorkingHours>().HasData(workingHours);

            base.OnModelCreating(modelBuilder);
        }
    }
}
