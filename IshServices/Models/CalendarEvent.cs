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
        public string ColorId { get; set; }

        public bool RegistrationAllowed
        {
            get
            {
                if (Description.IndexOf("register online", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    return true;
                }

                return false;
            }
        }

        public string RegistrationName
        {
            get
            {
                return Title + " - " + FormattedStartDate + " " + FormattedTimeDuration;
            }
        }

        private static TimeZoneInfo _est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        private static DateTime ToEst(DateTime serverTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(serverTime.ToUniversalTime(), _est);
        }

        public string FormattedStartDate
        {
            get
            {
                if (StartTime == null)
                {
                    return "Pending";
                }

                return ToEst(StartTime.Value).ToString("ddd, MMM d");
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
                    return ToEst(StartTime.Value).ToString("h.mmtt");
                }

                return string.Format("{0:h.mmtt} - {1:h.mmtt}", ToEst(StartTime.Value), ToEst(EndTime.Value));
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

        public bool IncludeOnHomeEvents()
        {
            if (FormattedStartDate == "Pending")
            {
                return false;
            }
            
            return true;
        }
    }
}