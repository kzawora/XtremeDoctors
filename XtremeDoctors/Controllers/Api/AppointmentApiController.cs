using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtremeDoctors.Helpers;
using XtremeDoctors.Models;
using XtremeDoctors.Services;

namespace XtremeDoctors.Controllers.Api
{
    [Route("api/appointment")]
    [ApiController]
    [Authorize]
    public class AppointmentApiController : ControllerBase
    {
        private AppointmentService appointmentService;
        private UserService userService;

        public AppointmentApiController(AppointmentService appointmentService, UserService userService)
        {
            this.appointmentService = appointmentService;
            this.userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        public ActionResult<IEnumerable<Appointment>> List()
        {
            return Ok(appointmentService.GetAllAppointments());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Appointment>>> MakeAppointmentAsync([FromQuery] int patientId, [FromQuery] int doctorId, [FromQuery] DateTime date, [FromQuery] string hour, [FromQuery] string comment = "")
        {
            var appointment = appointmentService.MakeAppointment(doctorId, patientId, date, hour, comment);
            if (appointment == null)
                return BadRequest();
            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, appointment.PatientId))
            {
                return Forbid();
            }
            return Ok(appointment);
        }


        [HttpDelete("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Appointment>>> DeleteAppointmentAsync(int appointmentId)
        {
            Appointment app = appointmentService.GetAppointmentById(appointmentId);
            if (app is null)
            {
                return NotFound();
            }

            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, app.PatientId))
            {
                return Forbid();
            }
            return Ok(appointmentService.CancelAppointmentById(appointmentId));
        }


        [HttpGet("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentAsync(int appointmentId)
        {
            Appointment app = appointmentService.GetAppointmentById(appointmentId);
            if (app is null)
            {
                return NotFound();
            }

            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, app.PatientId))
            {
                return Forbid();
            }
            return Ok(appointmentService.GetAppointmentById(appointmentId));
        }

        [HttpPut("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Appointment>>> EditAppointmentAsync(int appointmentId, [FromBody] Appointment appointment)
        {
            Appointment app = appointmentService.GetAppointmentById(appointmentId);
            if (app is null)
            {
                return NotFound();
            }

            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, app.PatientId))
            {
                return Forbid();
            }
            Appointment response = appointmentService.EditAppointmentById(appointmentId, appointment);
            if (response == null)
                return BadRequest();
            return Ok(response);
        }

        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Appointment>>> ListForPatientAsync(int patientId)
        {
            if (!await RoleHelper.HasAccessToPatientSpecificDataAsync(User, userService, patientId))
            {
                return Forbid();
            }
            return Ok(appointmentService.GetAppointmentsForPatient(patientId));
        }

    }
}
