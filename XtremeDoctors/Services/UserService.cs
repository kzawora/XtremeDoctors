using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using XtremeDoctors.Data;
using XtremeDoctors.Models;
using Microsoft.AspNetCore.Http;

namespace XtremeDoctors.Services
{
    public class UserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private ApplicationDbContext database;
        public UserService(ApplicationDbContext database, UserManager<User> userManager, IHttpContextAccessor _httpContextAccessor)
        {
            this.database = database;
            this._userManager = userManager;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public async Task<User> GetCurrentUser()
        {
            User user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            return user;
        }

        public async Task<int> GetCurrentPatientId()
        {
            User user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            int patientId = 1;

            if (user.Patient != null)
            {
                patientId = user.Patient.Id;
            }

            return patientId;
        }

    }
}
