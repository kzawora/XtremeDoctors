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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("[controller]")]
    public class PatientController : Controller
    {
        private PatientService patientService;

        public PatientController(PatientService patientService)
        {
            this.patientService = patientService;
        }

        [HttpGet("")]
        [Authorize(Roles=Roles.AdminReceptionist)]
        public IActionResult List()
        {
            ViewBag.patients = patientService.GetPatients();
            return View();
        }

        [HttpGet("create")]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public IActionResult Create([FromForm] string firstname, [FromForm] string surname)
        {
            patientService.AddPatient(firstname, surname);
            return RedirectToAction("", "Patient");
        }
    }
}