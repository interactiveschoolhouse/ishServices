using IshServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Validators
{
    public class PhoneValidator : Validator<RegistrationRequest>
    {

        private bool HasValidDigitCount(string phoneNumber)
        {
            int numberCount = 0;
            foreach(char digit in phoneNumber)
            {
                if (Char.IsDigit(digit))
                {
                    numberCount++;
                }
            }

            return numberCount == 10 || numberCount == 7;
        }
        public ValidatorResult Validate(RegistrationRequest target)
        {
            if (string.IsNullOrWhiteSpace(target.Phone) || !HasValidDigitCount(target.Phone))
            {
                return ValidatorResult.Error("Phone", "A phone number is required");
            }

            return ValidatorResult.Ok();
        }
    }
}