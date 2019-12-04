using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XtremeDoctors.Helpers;
using XtremeDoctors.Models;
using XtremeDoctors.Data;
using XtremeDoctors.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace XtremeDoctors.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        private readonly ILogger logger;
        private UserService userService;
        private readonly AppointmentService appointmentService;
        public AppointmentController(AppointmentService appointmentService, UserService userService, ILogger<AppointmentController> logger)
        {
            this.appointmentService = appointmentService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("")]
        [Authorize(Roles=Roles.Patient)]
        public async Task<IActionResult> ListForCurrentPatient()
        {
            int? patientId = await userService.GetCurrentPatientIdAsync();
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(patientId.Value);

            logger.LogInformation("List of appointments for patient with id {patientId} is being displayed", patientId.Value);

            return View();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles=Roles.AdminReceptionist)]
        public IActionResult ListForPatient(int id)
        {
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(id);
            ViewBag.patientId = id;
            return View("List");
        }

        [HttpGet("view/{id:int}")]
        public async Task<IActionResult> View(int id)
        {
            Appointment appointment = appointmentService.GetAppointmentById(id);
            if(appointment is null)
            {
                return new StatusCodeResult(404);
            }

            int? patientId = await userService.GetCurrentPatientIdAsync();
            if (patientId is null || patientId != appointment.Patient.Id)
            {
                return Forbid();
            }

            ViewBag.appointment = appointment;

            logger.LogInformation("Appointment with id {appId} is being displayed", appointment.Id);

            return View();
        }

        [HttpGet("pdf")]
        [Authorize(Roles = Roles.Patient)]
        public async Task<IActionResult> GeneratePdfAsync()
        {
            int? patientId = await userService.GetCurrentPatientIdAsync();
            return await GeneratePdfByIdAsync(patientId.Value);
        }

        [HttpGet("{id:int}/pdf")]
        public async Task<IActionResult> GeneratePdfByIdAsync(int id)
        {
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(id);
            var viewHtml = await this.RenderViewAsync("Report", View().Model, true);
            var pdf = WebHelper.PdfSharpConvert(viewHtml);
            var content = new System.IO.MemoryStream(pdf);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "report.pdf";
            return File(content, contentType, fileName);
        }

        [HttpPost("updateComment/{id:int}")]
        public IActionResult UpdateComment(int id, [FromForm] string comment)
        {
            Appointment editedAppointment = appointmentService.UpdateAppointmentComment(id, comment);
            
            logger.LogInformation("Appointment with id {appId} was edited", id);

            return RedirectToAction("ListPerPatient", new { id = editedAppointment.Patient.Id });
        }


        [HttpPost("create")]
        public async Task<IActionResult> Make(
            [FromForm] int doctorId,
            [FromForm] DateTime date,
            [FromForm] int? patient,
            [FromForm] string hour)
        {
            if (patient == null)
            {
                patient = await userService.GetCurrentPatientIdAsync();
                if (patient == null)
                {
                    return Forbid();
                }
            }
            
            appointmentService.MakeAppointment(doctorId, patient.Value, date, hour, "");
            return RedirectToAction("ListPerPatient", new { id=patient.Value });
        }

        [HttpGet("cancel/{appointmentId:int}")]
        public IActionResult Cancel(int appointmentId)
        {
            Appointment canceledAppointment = appointmentService.CancelAppointmentById(appointmentId);

            logger.LogInformation("Appointment with id {appId} was canceled", appointmentId);

            return RedirectToAction("ListPerPatient", "Appointment", new { id = canceledAppointment.Patient.Id });
        }
    }
}
