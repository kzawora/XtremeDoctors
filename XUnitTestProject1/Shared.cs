using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using XtremeDoctors.Data;
using XtremeDoctors.Models;
using XtremeDoctors.Services;
using Xunit;

namespace XtremeDoctorsUnitTests
{
    public static class Shared
    {
        public static ApplicationDbContext GetDatabaseContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            ApplicationDbContext databaseContext = new ApplicationDbContext(options);
            return databaseContext;
        }
    }
}
