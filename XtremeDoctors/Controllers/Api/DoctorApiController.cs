using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Services;
using XtremeDoctors.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using XtremeDoctors.Data;

namespace XtremeDoctors.Controllers.Api
{
    [Route("api/doctor")]
    [ApiController]
    [Authorize]
    public class DoctorApiController : ControllerBase
    {
        private DoctorService doctorService;
        public DoctorApiController(DoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Doctor>> List()
        {
            return Ok(doctorService.FindAllDoctors());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<IEnumerable<Doctor>> CreateNew([FromBody] Doctor doctor)
        {
            if (doctor is null)
                return NotFound();
            if (doctor.Id != 0)
                return BadRequest();
            return Ok(doctorService.AddDoctor(doctor));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Doctor> Get(int id)
        {
            Doctor result = doctorService.FindDoctor(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<Doctor> EditDoctor(int id, [FromBody] Doctor doctor)
        {
            Doctor old = doctorService.FindDoctor(id);
            if (old == null)
            {
                return NotFound();
            }
            Doctor result = doctorService.EditDoctor(old, doctor);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<Doctor> DeleteDoctor(int id)
        {
            Doctor doctor = doctorService.FindDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctorService.RemoveDoctor(doctor));
        }

        [HttpGet("{id}/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<string[]> GetAvailableDays(int id, [FromQuery] DateTime? date = null, [FromQuery] int daysForward = 14)
        {
            var actualDate = date ?? DateTime.Now;
            return Ok(doctorService.GetAvailableDays(doctorService.FindDoctor(id), actualDate, daysForward));
        }

        [HttpGet("{id}/working_hours")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<WorkingHours[]> GetWorkingHours(int id)
        {
            var result = doctorService.GetWorkingHoursForDoctorId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("{id}/working_hours")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<WorkingHours[]> AddWorkingHours([FromBody] WorkingHours workingHours)
        {
            if (workingHours.Id != 0)
                return BadRequest();
            return Ok(doctorService.AddWorkingHours(workingHours));
        }

        [HttpGet("working_hours/{workingHoursId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<WorkingHours[]> GetWorkingHoursById(int workingHoursId)
        {
            var result = doctorService.GetWorkingHoursById(workingHoursId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("working_hours/{workingHoursId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<WorkingHours[]> EditWorkingHours(int doctorId, int workingHoursId, [FromBody] WorkingHours edited)
        {
            WorkingHours old = doctorService.GetWorkingHoursById(workingHoursId);
            if (old == null)
            {
                return NotFound();
            }
            WorkingHours result = doctorService.EditWorkingHours(old, edited);
            return Ok(result);
        }

        [HttpDelete("working_hours/{workingHoursId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<WorkingHours> DeleteWorkingHours(int workingHoursId)
        {
            WorkingHours result = doctorService.RemoveWorkingHours(workingHoursId);
            return Ok(result);
        }

        [HttpGet("{id}/available_hours")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string[]> GetAvailableHours(int id, [FromQuery] DateTime? date = null)
        {
            Doctor doctor = doctorService.FindDoctor(id);
            var actualDate = date ?? DateTime.Now;
            return Ok(doctorService.GetFreeHoursForDate(doctor, actualDate));
        }
    }
}
