﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using Microsoft.AspNetCore.Authorization;

namespace XtremeDoctors.Controllers
{
    public class DoctorViewModel
    {
        public DoctorViewModel(Doctor doctor, DoctorService doctorService)
        {
            this.doctor = doctor;
            this.workingHours = doctorService.GetHoursStringForWholeWeek(doctor);
        }

        public Doctor doctor;
        public string[] workingHours;
    }

    [Authorize]
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        private DoctorService doctorService;
        public DoctorController(DoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [HttpGet("")]
        public IActionResult List()
        {
            ViewBag.doctorViews = doctorService
                .FindAllDoctors()
                .Select(doctor => new DoctorViewModel(doctor, doctorService))
                .ToArray();
            return View();
        }

        [HttpGet("{id:int}")]
        public IActionResult View(int id,
            [FromQuery(Name = "date")] DateTime date)
        {
            Doctor doctor = doctorService.FindDoctor(id);
            if (doctor == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            DoctorViewModel viewModel = new DoctorViewModel(doctor, doctorService);

            ViewBag.doctorView = viewModel;
            ViewBag.freeHours = doctorService.ComputeFreeSlots(doctor, date);
            ViewBag.date = date;

            return View();
        }


    }
}