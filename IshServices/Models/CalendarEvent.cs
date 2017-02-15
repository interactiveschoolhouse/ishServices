using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }

    }
}