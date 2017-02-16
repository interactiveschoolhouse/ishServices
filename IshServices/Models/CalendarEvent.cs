using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IshServices.Models
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string FormattedDescription
        {
            get
            {
                return Regex.Replace(Description, @"[\r\n]+", "<br/>");
            }
        }
    }
}