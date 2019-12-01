using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using XtremeDoctors.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace XtremeDoctors.Controllers
{
    public class DoctorViewModel
    {
        public Doctor doctor;
        public string[] workingHours;
    }

    [Authorize]
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        private DoctorService doctorService;
        public DoctorController(DoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [HttpGet("")]
        public IActionResult List()
        {
            ViewBag.doctors = doctorService.FindAllDoctors();
            return View();
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> generatePdfAsync()
        {
            ViewBag.doctors = doctorService.FindAllDoctors();
            var viewHtml = await this.RenderViewAsync("~/Views/Doctor/List.cshtml", View().Model, true);
            var pdf = WebHelper.PdfSharpConvert(viewHtml);
            var content = new System.IO.MemoryStream(pdf);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "report.pdf";
            return File(content, contentType, fileName);
        }

        [HttpGet("{id:int}")]
        public IActionResult View(int id,
            [FromQuery(Name = "date")] DateTime date)
        {
            Doctor doctor = doctorService.FindDoctor(id);
            if (doctor == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            DoctorViewModel viewModel = new DoctorViewModel {
                doctor = doctor,
                workingHours = doctorService.GetHoursStringForWholeWeek(doctor),
            };

            ViewBag.doctorView = viewModel;
            ViewBag.freeHours = doctorService.ComputeFreeSlots(doctor, date);
            ViewBag.date = date;

            return View();
        }


    }
}