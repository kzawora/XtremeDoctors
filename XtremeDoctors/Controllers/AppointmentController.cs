using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Services;

namespace XtremeDoctors.Controllers
{
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        private readonly AppointmentService appointmentService;
        public AppointmentController(AppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet("")]
        public IActionResult List()
        {
            int currentPatientId = 1; // TODO
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(currentPatientId);
            return View();
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] int doctorId,
            [FromBody] DateTime date,
            [FromBody] string hour)
        {
            //Doctor doctor = doctorService.FindDoctor(doctorId);



            return View();
        }
    }
}