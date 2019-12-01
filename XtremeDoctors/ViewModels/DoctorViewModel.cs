using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XtremeDoctors.Models;
using XtremeDoctors.Services;

namespace XtremeDoctors.ViewModels
{
    public class DoctorViewModel
    {
        public DoctorViewModel(Doctor doctor, DoctorService doctorService)
        {
            this.doctor = doctor;
            this.workingHours = doctorService.GetHoursStringForWholeWeek(doctor);
        }

        public Doctor doctor;
        public string[] workingHours;
    }
}
