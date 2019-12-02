using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using XtremeDoctors.Data;
using XtremeDoctors.Models;
using Microsoft.AspNetCore.Http;

namespace XtremeDoctors.Services
{
    public class UserService
    {

        private IHttpContextAccessor _httpContextAccessor;
        private UserManager<User> _userManager;
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
            User user = await GetCurrentUser();

            if(user == null)
            {
                return 1; //Temp
            }

            if (!user.PatientId.HasValue)
            {
                Patient patient = new Patient();
                patient.Name = "Jan";
                patient.Surname = "Kowalski";


                database.Patients.Add(patient);
                database.SaveChanges();

                user.PatientId = patient.Id;

                var result = await _userManager.UpdateAsync(user); // This does not work kurwa jego mać!

                if (!result.Succeeded)
                {
                    return 1; //Temp
                }

            }

            return user.PatientId.Value;
        }

    }
}
