using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XtremeDoctors.Helpers;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using Microsoft.AspNetCore.Mvc.Routing;

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
            int? patientId = await userService.GetCurrentPatientIdAsync();
            if (!patientId.HasValue)
            {
                return Forbid();
            }
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(patientId.Value);
            return View();
        }

        [HttpGet("{id:int}")]
        public IActionResult ListPerPatient(int id)
        {
            ViewBag.appointments = appointmentService.GetAppointmentsForPatient(id);
            ViewBag.patientId = id;
            return View("List");
        }

        [HttpGet("view/{id:int}")]
        public IActionResult View(int id)
        {
            Appointment appointment = appointmentService.GetAppointmentById(id);
            if(appointment is null){
                return new StatusCodeResult(404);
            }
            ViewBag.appointment = appointment;
            return View();
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GeneratePdfAsync()
        {
            int? patientId = await userService.GetCurrentPatientIdAsync();
            if (!patientId.HasValue)
            {
                return Forbid();
            }
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
            return RedirectToAction("ListPerPatient", "Appointment", new { id = canceledAppointment.Patient.Id });
        }
    }
}
