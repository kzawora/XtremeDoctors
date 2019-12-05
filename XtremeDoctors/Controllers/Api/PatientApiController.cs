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
    [Route("api/patient")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        private PatientService patientService;

        public PatientApiController(PatientService patientService)
        {
            this.patientService = patientService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<IEnumerable<Patient>> List()
        {
            return patientService.GetPatients();
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<Patient> Create([FromBody] Patient patient)
        {
            if (patient.Id != 0)
                return BadRequest();
            return patientService.AddPatient(patient);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<Patient> DeletePatient(int id)
        {
            return patientService.RemovePatient(id);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<Patient> GetPatient(int id)
        {
            return patientService.GetPatient(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.AdminReceptionist)]
        public ActionResult<Patient> EditPatient(int id, [FromBody] Patient patient)
        {
            Patient old = patientService.GetPatient(id);
            if (old == null)
            {
                return NotFound();
            }
            Patient result = patientService.EditPatient(old, patient);
            return Ok(result);
        }
    }
}
