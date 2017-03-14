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
                _cachedEvents = new ExpiringCache<IEnumerable<CalendarEvent>>(GetEvents(null, null), 
                    TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["GoogleCalendarMinutesToCacheEvents"])));
            }
            else if(_cachedEvents.IsExpired())
            {
                _cachedEvents.Refresh(GetEvents(null, null));
            }

            return _cachedEvents.Item;
        }

        [HttpGet]
        [Route("api/calendar/filtered")]
        public IEnumerable<CalendarEvent> GetFilteredEvents(int numToTake, bool registerEventsOnly)
        {
            return GetEvents(numToTake, registerEventsOnly);
        }

        private static IEnumerable<CalendarEvent> GetEvents(int? numToTake, bool? registerEventsOnly)
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey
            });

            EventsResource.ListRequest request = service.Events.List(_calendarID);
            request.TimeMin = DateTime.UtcNow;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = int.Parse(ConfigurationManager.AppSettings["TakeGoogleCalendarItemsCount"]);
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

            if (events.Items == null)
            {
                return new CalendarEvent[] { };
            }
            var eventsQuery = events.Items
                .Where(evt => !string.IsNullOrEmpty(evt.Description))
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

            if (registerEventsOnly.GetValueOrDefault())
            {
                eventsQuery = eventsQuery.Where(evt => evt.RegistrationAllowed == true);
            }

            return eventsQuery.Take(numToTake.GetValueOrDefault(int.Parse(ConfigurationManager.AppSettings["TakeCalendarItemsCount"])));
        }

        // GET api/<controller>/5
        public CalendarEvent Get(string id)
        {
            return new CalendarEvent();
        }

    }
}