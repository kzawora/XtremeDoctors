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
    }
}
