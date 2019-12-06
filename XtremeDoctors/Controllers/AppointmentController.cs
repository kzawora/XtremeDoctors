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
    [ApiExplorerSettings(IgnoreApi = true)]
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

        [HttpGet("current")]
        [Authorize(Roles=Roles.Patient)]
        public async Task<IActionResult> ListForCurrentPatient()
        {
            int? patientId = await userService.GetCurrentPatientIdAsync();
            if (patientId == null)
            {
                // Not logged in user
                return Forbid();
            }

            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(patientId.Value);
            ViewBag.patientId = patientId.Value;
            logger.LogInformation("List of appointments for patient with id {patientId} is being displayed", patientId.Value);
            return View("List");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ListForPatient(int id)
        {
            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, id))
            {
                return Forbid();
            }

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
                return NotFound();
            }

            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, appointment.PatientId))
            {
                return Forbid();
            }

            ViewBag.appointment = appointment;
            logger.LogInformation("Appointment with id {appId} is being displayed", appointment.Id);
            return View();
        }

        [HttpGet("pdf/{patientId:int}")]
        public async Task<IActionResult> GeneratePdfById(int patientId)
        {
            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, patientId))
            {
                return Forbid();
            }

            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(patientId);
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

            return RedirectToAction("ListForPatient", new { id = editedAppointment.Patient.Id });
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
                    logger.LogError("There was en error while trying to identify the patient");
                    return Forbid();
                }
            }
            
            appointmentService.MakeAppointment(doctorId, patient.Value, date, hour, "");
            return RedirectToAction("ListForPatient", new { id=patient.Value });
        }

        [HttpGet("cancel/{appointmentId:int}")]
        public IActionResult Cancel(int appointmentId)
        {
            Appointment canceledAppointment = appointmentService.CancelAppointmentById(appointmentId);

            logger.LogInformation("Appointment with id {appId} was canceled", appointmentId);

            return RedirectToAction("ListForPatient", "Appointment", new { id = canceledAppointment.Patient.Id });
        }
    }
}
