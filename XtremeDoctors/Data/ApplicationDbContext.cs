using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using XtremeDoctors.Models;
using Microsoft.Extensions.Configuration;

namespace XtremeDoctors.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor
                {
                    Id = 1,
                    Name = "Dr",
                    Surname = "Etker",
                    Specialization = "Kisiele",
                    Text = "Very good doctor"
                },
                new Doctor
                {
                    Id = 2,
                    Name = "Dr",
                    Surname = "Pepper",
                    Specialization = "Drinking",
                    Text = "Not so good doctor"
                },
                new Doctor
                {
                    Id = 3,
                    Name = "Dr",
                    Surname = "Dre",
                    Specialization = "Wrapping",
                    Text = "Quite good doctor."
                }
            );

            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    Id = 1,
                    Name = "First",
                    Surname = "Patient"
                },
                new Patient
                {
                    Id = 2,
                    Name = "Second",
                    Surname = "Patient"
                }
            );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    Start = new DateTime(3),
                    End = new DateTime(3),
                    Patient = null,
                    Doctor = null,
                    RoomNumber = 3,
                },
                new Appointment
                {
                    Id = 2,
                    Start = new DateTime(3),
                    End = new DateTime(3),
                    Patient = null,
                    Doctor = null,
                    RoomNumber = 3,
                }
            );
        }
    }
}
