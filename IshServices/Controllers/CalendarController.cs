using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using IshServices.Auth;
using IshServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IshServices.Controllers
{
    [ApiKeyAuth]
    public class CalendarController : ApiController
    {

        private static string _calendarID = ConfigurationManager.AppSettings["CalendarID"];
        private static string _apiKey = ConfigurationManager.AppSettings["CalendarApiKey"];
        private static ExpiringCache<IEnumerable<CalendarEvent>> _cachedEvents = null;
        // GET api/<controller>
        public IEnumerable<CalendarEvent> Get()
        {
            if (_cachedEvents == null)
            {
                _cachedEvents = new ExpiringCache<IEnumerable<CalendarEvent>>(GetEvents(), 
                    TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["GoogleCalendarMinutesToCacheEvents"])));
            }
            else if(_cachedEvents.IsExpired())
            {
                _cachedEvents.Refresh(GetEvents());
            }

            return _cachedEvents.Item;
        }

        private static IEnumerable<CalendarEvent> GetEvents()
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey
            });

            EventsResource.ListRequest request = service.Events.List(_calendarID);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = int.Parse(ConfigurationManager.AppSettings["TakeGoogleCalendarItemsCount"]);
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

            if (events.Items == null)
            {
                return new CalendarEvent[] { };
            }
            return events.Items
                .Where(evt => !string.IsNullOrEmpty(evt.Description))
                .Take(int.Parse(ConfigurationManager.AppSettings["TakeCalendarItemsCount"]))
                .Select(evt =>
                new CalendarEvent()
                {
                    ColorId = evt.ColorId,
                    Title = evt.Summary,
                    Description = evt.Description,
                    StartTime = evt.Start.DateTime,
                    EndTime = evt.End.DateTime,
                    Location = evt.Location
                })
                .Where(ce => ce.IncludeOnHomeEvents());
        }

        // GET api/<controller>/5
        public CalendarEvent Get(string id)
        {
            return new CalendarEvent();
        }

    }
}