using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XtremeDoctors.Models;
using Microsoft.Extensions.Configuration;

namespace XtremeDoctors.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        DbSet<Doctor> Doctors { get; set; }
    }
}
