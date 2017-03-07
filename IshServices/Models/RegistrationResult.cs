using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class RegistrationResult
    {
        private RegistrationResult()
        {

        }
        public long RegistrationId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address1 { get; private set; }
        public string City { get; private set; }
        public string PhoneAreaCode { get; private set; }
        public string PhonePrefix { get; private set; }
        public string PhoneSuffix { get; private set; }
        public string Email { get; private set; }

        public decimal PaymentAmount { get; private set; }

        public bool IsValid { get; private set; }

        public IEnumerable<FieldError> Errors { get; set; } = new List<FieldError>();

        public static RegistrationResult Process(CustomerRegistration request)
        {
            //TODO: validate, unit test this
            return new RegistrationResult();
        }

    }
}