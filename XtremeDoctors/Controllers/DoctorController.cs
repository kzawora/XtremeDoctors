using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;
using XtremeDoctors.Data;

namespace XtremeDoctors.Controllers
{
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        private ApplicationDbContext database;
        public DoctorController(ApplicationDbContext database)
        {
            this.database = database;
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            Doctor[] doctors = new Doctor[2];

            database.Doctors.Add(new Doctor("Dr", "Dre", "Internist", "Cheap and good"));
            database.SaveChanges();

            ViewData["Message"] = "Your doctors page.";
            ViewBag.doctors = database.Doctors.ToArray();
            return View();
        }

        [HttpGet("view/{id:int}")]
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