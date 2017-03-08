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

        public bool IsValid
        {
            get
            {
                return Errors.Count > 0;
            }
        }

        public List<FieldError> Errors { get; set; } = new List<FieldError>();

        public bool HasError(string errorKey)
        {
            return Errors.Any(err => err.Name == errorKey);
        }

        public void ParseName(string name)
        {
            int lastNameIndex = name.IndexOf(" ");

            if (lastNameIndex != -1)
            {
                FirstName = name.Substring(0, lastNameIndex).Trim();
                LastName = name.Substring(lastNameIndex).Trim();
            }
            else
            {
                FirstName = name.Trim();
            }
        }

        public static RegistrationResult Process(RegistrationRequest request)
        {
            RegistrationResult result = new RegistrationResult();
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                result.Errors.Add(new FieldError("Name", "Name is required"));
            }
            else
            {
                result.ParseName(request.Name);
            }

            if (request.PaymentAmount <= 0)
            {
                result.Errors.Add(new FieldError("PaymentAmount", "Payment amount is required"));
            }

            return result;
        }

        public string GetError(string key)
        {
            var error = Errors.Find(err => err.Name == key);

            if (error != null)
            {
                return error.Description;
            }

            return null;
        }
    }
}