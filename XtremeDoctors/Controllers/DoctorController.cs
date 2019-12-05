using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using XtremeDoctors.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace XtremeDoctors.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DoctorController : Controller
    {

        private readonly ILogger logger;
        private DoctorService doctorService;
        public DoctorController(DoctorService doctorService, ILogger<DoctorController> logger)
        {
            this.doctorService = doctorService;
            this.logger = logger;
        }

        [HttpGet("")]
        public IActionResult List([FromQuery(Name = "patient")] int? patientId)
        {
            ViewBag.doctorViews = doctorService
                .FindAllDoctors()
                .Select(doctor => new DoctorViewModel(doctor, doctorService))
                .ToArray();
            ViewBag.patientId = patientId;
            return View();
        }

        [HttpGet("{id:int}")]
        public IActionResult View(int id,
            [FromQuery(Name = "date")] DateTime date,
            [FromQuery(Name = "patient")] int? patientId
            )
        {
            Doctor doctor = doctorService.FindDoctor(id);
            if (doctor == null)
            {
                logger.LogError("There was en error while trying to find doctor");
                Response.StatusCode = 404;
                return View("NotFound");
            }

            DoctorViewModel viewModel = new DoctorViewModel(doctor, doctorService);

            ViewBag.doctorView = viewModel;
            ViewBag.freeHours = doctorService.ComputeFreeSlots(doctor, date);
            ViewBag.date = date;
            ViewBag.patientId = patientId;
            return View();
        }


    }
}