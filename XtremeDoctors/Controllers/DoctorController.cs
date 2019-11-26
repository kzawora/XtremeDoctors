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
            ViewData["Message"] = "Your doctors page.";
            ViewBag.doctors = database.Doctors.ToArray();
            return View();
        }

        [HttpGet("{id:int}/{day:int}/{week:int}/{year:int}")]
        public IActionResult View(int id, int day, int week, int year)
        {
            Doctor doctor = database.Doctors.Find(id);
            if (doctor == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            ViewBag.doctor = doctor;

            int[] freeSlots = new int[16];

            for(int i = 0; i < 16; i++)
            {
                freeSlots[i] = i + 5;
            }



            ViewBag.freeSlots = freeSlots;

            return View();
        }
    }
}