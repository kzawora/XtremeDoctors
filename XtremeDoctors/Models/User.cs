using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtremeDoctors.Models
{
    public class User : IdentityUser
    {
        [ForeignKey("ReceptionistId")]
        public Receptionist Receptionist { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
