using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("{id:int}")]
        public IActionResult List(int id)
        {
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(id);
            return View();
        }

        [HttpPost]
        public IActionResult Make(
            [FromForm] int doctorId,
            [FromForm] DateTime date,
            [FromForm] string hour)
        {

            ViewBag.appointment = appointmentService.MakeAppointment(doctorId, date, hour);

            return View();
        }
    }
}