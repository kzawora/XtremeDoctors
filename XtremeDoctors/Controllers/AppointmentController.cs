using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XtremeDoctors.Models;
using XtremeDoctors.Services;

namespace XtremeDoctors.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AppointmentController : Controller
    {

        private UserService userService;
        private readonly AppointmentService appointmentService;
        public AppointmentController(AppointmentService appointmentService, UserService userService)
        {
            this.appointmentService = appointmentService;
            this.userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> List()
        {

            User user = await userService.GetCurrentUser();

            int patientId = 1;

            if (user.Patient != null)
            {
                patientId = user.Patient.Id;
            }

            //return RedirectToRoute(new { controller = "Home", action = "Index" });

            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(patientId);
            return View();
        }

        [HttpGet("{id:int}")]
        public IActionResult List(int id)
        {
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Make(
            [FromForm] int doctorId,
            [FromForm] DateTime date,
            [FromForm] string hour)
        {

            User user = await userService.GetCurrentUser();

            int patientId = 1;

            if (user.Patient != null)
            {
                patientId = user.Patient.Id;
            }

            ViewBag.appointment = appointmentService.MakeAppointment(doctorId, 1, date, hour);

            return View();
        }
    }
}