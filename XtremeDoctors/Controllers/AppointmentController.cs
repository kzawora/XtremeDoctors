using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XtremeDoctors.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}