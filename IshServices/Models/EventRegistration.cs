using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class EventRegistration
    {
        public long RegistrationId { get; set; }
        public RegistrationRequest CustomerInfo { get; set; }
        public DateTime Created { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}