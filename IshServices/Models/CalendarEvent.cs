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

        public bool RegistrationAllowed { get; private set; } = true;

        public string FormattedStartDate
        {
            get
            {
                if (StartTime == null)
                {
                    return "Pending";
                }

                return StartTime.Value.ToString("ddd, MMM d");
            }
        }

        public string FormattedTimeDuration
        {
            get
            {
                if (StartTime == null)
                {
                    return string.Empty;
                }

                if (EndTime == null)
                {
                    return StartTime.Value.ToString("h.mmtt");
                }

                return string.Format("{0:h.mmtt} - {1:h.mmtt}", StartTime.Value, EndTime.Value);
            }
        }

        public string[] DescriptionLines
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return new string[] { };
                }

                return Regex.Split(Description, @"[\r\n]+");
            }
        }
    }
}