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
            return string.Format("{0}:{1:D2}", hour, minute * 15);
        }
    }
}
