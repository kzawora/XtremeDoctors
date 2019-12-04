using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;
using XtremeDoctors.Data;
using Microsoft.AspNetCore.Authorization;
using XtremeDoctors.Services;
using System.Security.Claims;

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

        [HttpGet("{id}")]
        public IActionResult View(int id)
        {
            return View();
        }

        [HttpGet("")]
        [Authorize(Roles=Roles.AdminReceptionist)]
        public IActionResult List()
        {
            ViewBag.patients = database.Patients.ToArray();
            return View();
        }
    }
}