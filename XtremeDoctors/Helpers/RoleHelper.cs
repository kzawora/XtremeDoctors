using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XtremeDoctors.Models;
using XtremeDoctors.Data;
using XtremeDoctors.Services;
using System.Security.Claims;

namespace XtremeDoctors.Helpers
{
    public static class RoleHelper
    {
        /// <summary>
        /// Checks if a user trying to get access to patient-related resource is the patient (the owner) himself
        /// or has a administrative role like a receptionist
        /// </summary>
        public static async Task<bool> HasAccessToPatientSpecificDataAsync(ClaimsPrincipal claimsPrincipal, UserService userService, int patientIdOfDataOwner)
        {
            User user = await userService.GetUserByClaimsPrincipalAsync(claimsPrincipal);
            if (user == null)
            {
                // Not logged in user
                return false;
            }

            if (user.PatientId == null)
            {
                // Not a patient, has to have an administrative role
                return claimsPrincipal.IsInRole(Roles.Receptionist) || claimsPrincipal.IsInRole(Roles.Admin);
            }

            // Patient, has to be the owner of resource
            return user.PatientId == patientIdOfDataOwner;
        }
    }
}
