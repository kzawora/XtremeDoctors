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
        private readonly UserManager<User> _userManager;
        private readonly AppointmentService appointmentService;
        public AppointmentController(AppointmentService appointmentService, UserManager<User> userManager)
        {
            this.appointmentService = appointmentService;
            this._userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            int patientId = 1;

            if (user.Patient != null)
            {
                patientId = user.Patient.Id;
            }

            //return RedirectToRoute(new { controller = "Home", action = "Index" });

            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(1);
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

            User user = await _userManager.GetUserAsync(HttpContext.User);

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