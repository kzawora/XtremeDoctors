using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XtremeDoctors.Helpers
{
    public class SlotHelper
    {
        public static string SlotToHour(int slot) {
            int hour = slot / 4;
            int minute = slot % 4;
            return string.Format("{0:D2}:{1:D2}", hour, minute * 15);
        }

        public static int HourToSlot(string hour)
        {
            string[] hourSplitted = hour.Split(":");
            
            int h = 0;
            if (!Int32.TryParse(hourSplitted[0], out h))
            {
                h = -1;
            }

            int m = 0;
            if (!Int32.TryParse(hourSplitted[1], out m))
            {
                h = -1;
            }

            int slot = h * 4 + (m / 15);

            return slot;
        }
    }
}
