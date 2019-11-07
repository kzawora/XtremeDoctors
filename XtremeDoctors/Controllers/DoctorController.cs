﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;

namespace XtremeDoctors.Controllers
{
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        [HttpGet("list")]
        public IActionResult List()
        {
            Doctor[] doctors = new Doctor[2];

            doctors[0] = new Doctor("Dr", "Dre", "Internist", "Cheap and good");
            doctors[1] = new Doctor("Dr", "Pepper", "Internist", "Good");

            ViewData["Message"] = "Your doctors page.";
            ViewBag.doctors = doctors;
            return View();
        }

        [HttpGet("view/{id}")]
        public IActionResult View(int id)
        {
            Doctor doctor = new Doctor("Dr", "Dre", "Internist", "Cheap and good");
            ViewBag.doctor = doctor;

            DateTime[] freeSlots = new DateTime[32];

            for(int i = 0; i < 32; i++)
            {
                freeSlots[i] = new DateTime(2019, 11, 7, 8, 00, 00);
            }

            for (int i = 0; i < 32; i++)
            {
                freeSlots[i].AddMinutes(i * 15);
            }

            ViewBag.freeSlots = freeSlots;

            return View();
        }
    }
}