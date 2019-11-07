using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;

namespace XtremeDoctors.Controllers
{
    [Route("[controller]")]
    public class PatientController : Controller
    {
        [HttpGet("view/{id}")]
        public IActionResult View(int id)
        {
            Patient patient = new Patient("Jan", "Daciuk");
            ViewBag.patient = patient;
            return View();
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            Patient[] patients = new Patient[2];
            patients[0] = new Patient("Jan", "Daciuk");
            patients[1] = new Patient("January", "Daciuk");
            
            ViewBag.patients = patients;
            return View();
        }

        public IActionResult Index()
        {
            Patient patient = new Patient("Jan", "Daciuk");
            ViewBag.patient = patient;
            return View();
        }
    }
}