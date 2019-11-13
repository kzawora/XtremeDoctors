using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XtremeDoctors.Controllers
{
    public class ErrorStatusCodesController : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("error/403")]
        public IActionResult Error403()
        {
            return View();
        }
    }
}