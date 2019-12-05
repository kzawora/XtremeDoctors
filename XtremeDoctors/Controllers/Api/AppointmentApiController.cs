using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Services;
using XtremeDoctors.Models;
using Microsoft.AspNetCore.Http;

namespace XtremeDoctors.Controllers.Api
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentApiController : ControllerBase
    {
        private AppointmentService appointmentService;
        public AppointmentApiController(AppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Appointment>> List()
        {
            return Ok(appointmentService.GetAllAppointments());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Appointment>> MakeAppointment([FromQuery] int patientId, [FromQuery] int doctorId, [FromQuery] DateTime date, [FromQuery] string hour, [FromQuery] string comment = "")
        {
            var appointment = appointmentService.MakeAppointment(doctorId, patientId, date, hour, comment);
            if (appointment == null)
                return BadRequest();
            return Ok(appointment);
        }


        [HttpDelete("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Appointment>> DeleteAppointment(int appointmentId)
        {
            return Ok(appointmentService.CancelAppointmentById(appointmentId));
        }


        [HttpGet("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Appointment>> GetAppointment(int appointmentId)
        {
            return Ok(appointmentService.GetAppointmentById(appointmentId));
        }

        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Appointment>> ListForPatient(int patientId)
        {
            return Ok(appointmentService.GetAppointmentsForPatient(patientId));
        }

    }
}
