using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtremeDoctors.Models
{
    public class Receptionist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [InverseProperty("Receptionist")]
        public User User { get; set; }
    }
}
