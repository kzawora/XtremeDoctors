using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;

namespace XtremeDoctors.Controllers
{
    public class DoctorsController : Controller
    {
        public IActionResult Doctors()
        {
            Doctor doctor = new Doctor("Dr", "Dre", "Internist");

            ViewData["Message"] = "Your doctors page.";
            return View(doctor);
        }
    }
}