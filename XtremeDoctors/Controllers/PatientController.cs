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
    public class PatientController : Controller
    {
        private ApplicationDbContext database;
        public PatientController(ApplicationDbContext database)
        {
            this.database = database;
        }

        [HttpGet("view/{id}")]
        public IActionResult View(int id)
        {
            return View();
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            ViewBag.patients = database.Patients.ToArray();
            return View();
        }
    }
}