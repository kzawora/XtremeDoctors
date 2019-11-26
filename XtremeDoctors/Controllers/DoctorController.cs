﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using XtremeDoctors.Helpers;

namespace XtremeDoctors.Controllers
{
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
            ViewData["Message"] = "Your doctors page.";
            ViewBag.doctors = doctorService.FindAllDoctors();
            return View();
        }

        [HttpGet("{id:int}/{day:int}/{week:int}/{year:int}")]
        public IActionResult View(int id, int day, int week, int year)
        {
            Doctor doctor = doctorService.FindDoctor(id);

            if (doctor == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            ViewBag.doctor = doctor;

            ViewBag.freeHours = doctorService.ComputeFreeSlots(doctor.Id);

            return View();
        }
    }
}