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
        public IActionResult List()
        {

            Doctor[] doctors = new Doctor[2];

            doctors[0] = new Doctor("Dr", "Dre", "Internist", "Cheap and good");
            doctors[1] = new Doctor("Dr", "Pepper", "Internist", "Dupa");

            ViewData["Message"] = "Your doctors page.";
            ViewBag.doctors = doctors;
            return View();
        }
    }
}