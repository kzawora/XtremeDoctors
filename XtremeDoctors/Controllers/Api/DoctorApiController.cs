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
    [Route("api/doctor")]
    [ApiController]
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
        public ActionResult<Doctor> DeleteDoctor(int id)
        {
            Doctor doctor = doctorService.FindDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctorService.RemoveDoctor(doctor));
        }


        [HttpGet("{id}/available_days")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string[]> GetAvailableDays(int id, [FromQuery] DateTime? date = null, [FromQuery] int daysForward = 14)
        {
            var actualDate = date ?? DateTime.Now;
            return Ok(doctorService.GetAvailableDays(doctorService.FindDoctor(id), actualDate, daysForward));
        }

        [HttpGet("{id}/available_hours")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string[]> GetAvailableHours(int id, [FromQuery] DateTime? date = null)
        {
            Doctor result = doctorService.FindDoctor(id);
            var actualDate = date ?? DateTime.Now;
            return Ok(doctorService.GetFreeHoursForDate(result, actualDate));
        }
    }
}
