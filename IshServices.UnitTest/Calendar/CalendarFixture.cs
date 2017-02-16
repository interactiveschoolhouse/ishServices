using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IshServices.Models;

namespace IshServices.UnitTest.Calendar
{
    [TestClass]
    public class CalendarFixture
    {
        [TestMethod]
        public void GetHtmlFormattedDescriptionWithBreakTags()
        {
            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Description = "line break" + Environment.NewLine + "it sure is"
            };

            Assert.AreEqual("line break<br/>it sure is", calendarEvent.FormattedDescription);

        }
    }
}
