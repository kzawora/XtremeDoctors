using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Services
{
    public class DoctorService
    {

        public int[] ComputeFreeSlots(int doctorId)
        {
            int[] freeSlots = new int[16];

            for (int i = 0; i < 16; i++)
            {
                freeSlots[i] = i + 5;
            }

            return freeSlots;
        }


    }
}
