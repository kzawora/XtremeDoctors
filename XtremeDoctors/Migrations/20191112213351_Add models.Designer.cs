﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XtremeDoctors.Data;

namespace XtremeDoctors.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191112213351_Add models")]
    partial class Addmodels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("XtremeDoctors.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DoctorId");

                    b.Property<DateTime>("End");

                    b.Property<int?>("PatientId");

                    b.Property<int>("RoomNumber");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("XtremeDoctors.Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Specialization");

                    b.Property<string>("Surname");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("XtremeDoctors.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("XtremeDoctors.Models.Receptionist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Receptionists");
                });

            modelBuilder.Entity("XtremeDoctors.Models.Appointment", b =>
                {
                    b.HasOne("XtremeDoctors.Models.Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId");

                    b.HasOne("XtremeDoctors.Models.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId");
                });
#pragma warning restore 612, 618
        }
    }
}
