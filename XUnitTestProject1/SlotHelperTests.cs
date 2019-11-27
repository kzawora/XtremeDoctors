using System;
using XtremeDoctors.Helpers;
using Xunit;

namespace XtremeDoctorsUnitTests
{
    public class SlotHelperUnitTests
    {
        [Fact]
        public void SlotToHour_Full_Day_Slots_Should_Equal_DateTime_Reference()
        {
            // Arrange
            // 1 int = 15 minute slot; 96 slots = 24h
            const int slotCount = 96;
            string[] fullDayDateTimeReference = new string[slotCount];
            string[] fullDaySlots = new string[slotCount];
            // Act
            for (int i = 0; i < slotCount; i++)
            {
                fullDaySlots[i] = SlotHelper.SlotToHour(i);
                fullDayDateTimeReference[i] = DateTime.Now.Date.AddMinutes(i * 15).ToString("HH:mm");
            }
            // Assert
            for (int i = 0; i < slotCount; i++)
            {
                Assert.Equal(fullDaySlots[i], fullDayDateTimeReference[i]);
            }
        }
        [Fact]
        public void HourToSlot_Full_Day_Slots_Should_Equal_DateTime_Reference()
        {
            // Arrange
            // 1 int = 15 minute slot; 96 slots = 24h
            const int slotCount = 96;
            string[] fullDayDateTimeReference = new string[slotCount];
            int[] fullDaySlots = new int[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                fullDayDateTimeReference[i] = DateTime.Now.Date.AddMinutes(i * 15).ToString("HH:mm");
            }
            // Act
            for (int i = 0; i < slotCount; i++)
            {
                fullDaySlots[i] = SlotHelper.HourToSlot(fullDayDateTimeReference[i]);
            }
            // Assert
            for (int i = 0; i < slotCount; i++)
            {
                Assert.Equal(fullDaySlots[i], i);
            }
        }
    }
}
