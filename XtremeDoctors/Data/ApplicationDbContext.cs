using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using XtremeDoctors.Models;
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
            modelBuilder.Entity<WorkingHours>().HasData(workingHours);

            modelBuilder.Entity<IdentityRole>().HasData(new[]
            {
                new { Id = Data.Roles.Patient, Name = Data.Roles.Patient, NormalizedName = Data.Roles.Patient },
                new { Id = Data.Roles.Receptionist, Name = Data.Roles.Receptionist, NormalizedName = Data.Roles.Receptionist },
                new { Id = Data.Roles.Admin, Name = Data.Roles.Admin, NormalizedName = Data.Roles.Admin },
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
