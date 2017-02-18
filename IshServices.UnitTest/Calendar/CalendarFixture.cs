using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Models;

namespace IshServices.UnitTest.Calendar
{
    [TestClass]
    public class CalendarFixture
    {
        [TestMethod]
        public void GetDescriptionLines()
        {
            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Description = "line break" + Environment.NewLine + "it sure is"
            };

            Assert.AreEqual(2, calendarEvent.DescriptionLines.Length);

            Assert.AreEqual("line break", calendarEvent.DescriptionLines[0]);
            Assert.AreEqual("it sure is", calendarEvent.DescriptionLines[1]);
        }

        [TestMethod]
        public void GetFormattedEventDate()
        {
            CalendarEvent calendarEvent = new CalendarEvent()
            {
                StartTime = new DateTime(2017, 2, 18, 10, 30, 45)
            };

            Assert.AreEqual("Sat, Feb 18", calendarEvent.FormattedStartDate);
        }
    
        [TestMethod]
        public void GetFormattedDurationWithStartTimeOnly()
        {
            CalendarEvent calendarEvent = new CalendarEvent()
            {
                StartTime = new DateTime(2017, 2, 18, 10, 30, 45)
            };

            Assert.AreEqual("10.30AM", calendarEvent.FormattedTimeDuration);
        }

        [TestMethod]
        public void GetFormattedDurationWithStartAndEndTime()
        {
            CalendarEvent calendarEvent = new CalendarEvent()
            {
                StartTime = new DateTime(2017, 2, 18, 10, 30, 45),
                EndTime = new DateTime(2017, 2, 18, 13, 45, 45)
            };

            Assert.AreEqual("10.30AM - 1.45PM", calendarEvent.FormattedTimeDuration);
        }
    }
}
