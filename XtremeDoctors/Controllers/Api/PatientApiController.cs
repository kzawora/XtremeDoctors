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
    [Route("api/patient")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        public PatientApiController()
        {
        }
    }
}
