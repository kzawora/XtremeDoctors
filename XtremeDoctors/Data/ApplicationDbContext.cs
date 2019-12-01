using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using XtremeDoctors.Models;

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
            var patients = new[]
            {
                new
                {
                    Id = 4,
                    Name = "First",
                    Surname = "Patient"
                },
                new
                {
                    Id = 5,
                    Name = "Second",
                    Surname = "Patient"
                }
            };
            var appointments = new[]
            {
                new
                {
                    Id = 6,
                    Date = DateTime.Now.Date,
                    StartSlot = 15,
                    EndSlot = 17,
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    RoomNumber = 3,
                },
                new
                {
                    Id = 7,
                    Date = DateTime.Now.Date,
                    StartSlot = 22,
                    EndSlot = 22,
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    RoomNumber = 3,
                }
            };
            var workingHours = new[]{
                new
                {
                    Id = 8,
                    Date = DateTime.Now.Date,
                    StartSlot = 10,
                    EndSlot = 50,
                    DoctorId = doctors[0].Id,
                },
                new
                {
                    Id = 9,
                    Date = DateTime.Now.Date,
                    StartSlot = 12,
                    EndSlot = 30,
                    DoctorId = doctors[1].Id,
                }
            };
            modelBuilder.Entity<Doctor>().HasData(doctors);
            modelBuilder.Entity<Patient>().HasData(patients);
            modelBuilder.Entity<Appointment>().HasData(appointments);
            modelBuilder.Entity<WorkingHours>().HasData(workingHours);
            base.OnModelCreating(modelBuilder);
        }
    }
}
