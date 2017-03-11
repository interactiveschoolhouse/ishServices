using IshServices.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class ClassRegistration
    {
        private ClassRegistration()
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

        public string SpecialInstructions { get; private set; }
        public string EventName { get; private set; }
        public string EventDescription { get; private set; }

        public bool IsValid
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        public List<ValidatorResult> Errors { get; private set; } = new List<ValidatorResult>();

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

        public static ClassRegistration Process(RegistrationRequest request, IEnumerable<Validator<RegistrationRequest>> validations)
        {
            ClassRegistration registrationResult = new ClassRegistration();
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                registrationResult.Errors.Add(ValidatorResult.Error("Name", "Name is required"));
            }

            if (validations != null)
            {
                foreach(var validator in validations)
                {
                    var validationResult = validator.Validate(request);

                    if (!validationResult.IsValid)
                    {
                        registrationResult.Errors.Add(validationResult);
                    }
                }
            }

            if (registrationResult.IsValid)
            {
                registrationResult.ParseName(request.Name);
                registrationResult.ParsePhone(request.Phone);
                registrationResult.PaymentAmount = request.PaymentAmount;
                registrationResult.Email = request.Email;
                registrationResult.Address1 = request.Address;
                registrationResult.City = request.Town;
                registrationResult.SpecialInstructions = request.SpecialInstructions;
                registrationResult.EventName = request.EventName;
                registrationResult.EventDescription = request.EventDescription;
            }

            return registrationResult;
        }

        private void ParsePhone(string phone)
        {
            if (phone == null)
            {
                return;
            }

            string digitsOnly = new string(phone.Where(ch => Char.IsDigit(ch)).ToArray());
            if (digitsOnly.Length == 7)
            {
                PhoneAreaCode = "413";
                PhonePrefix = digitsOnly.Substring(0, 3);
                PhoneSuffix = digitsOnly.Substring(3);
            }
            else
            {
                PhoneAreaCode = digitsOnly.Substring(0, 3);
                PhonePrefix = digitsOnly.Substring(3, 3);
                PhoneSuffix = digitsOnly.Substring(6);

            }
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