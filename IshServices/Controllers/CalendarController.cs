using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
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
    public class CalendarController : ApiController
    {

        private static string _calendarID = ConfigurationManager.AppSettings["CalendarID"];
        private static string _apiKey = ConfigurationManager.AppSettings["CalendarApiKey"];
        // GET api/<controller>
        public IEnumerable<CalendarEvent> Get()
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey
            });

            EventsResource.ListRequest request = service.Events.List(_calendarID);
            request.TimeMin = new DateTime(2017, 2, 1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 4;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

            if (events.Items == null)
            {
                return null;
            }

            return events.Items.Select(evt =>
                new CalendarEvent()
                {
                    Title = evt.Summary,
                    Description = evt.Description,
                    StartTime = evt.Start.DateTime,
                    EndTime = evt.End.DateTime
                }
            );

        }

        // GET api/<controller>/5
        public CalendarEvent Get(string id)
        {
            return new CalendarEvent();
        }

    }
}