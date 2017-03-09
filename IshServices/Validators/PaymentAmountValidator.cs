using IshServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Validators
{
    public class PaymentAmountValidator : Validator<RegistrationRequest>
    {
        public PaymentAmountValidator(decimal minPaymentAmount, decimal maxPaymentAmount)
        {
            MinPaymentAmount = minPaymentAmount;
            MaxPaymentAmount = maxPaymentAmount;
        }

        public decimal MinPaymentAmount { get; private set; }
        public decimal MaxPaymentAmount { get; private set; }

        public ValidatorResult Validate(RegistrationRequest target)
        {
            if (target.PaymentAmount < MinPaymentAmount || target.PaymentAmount > MaxPaymentAmount)
            {
                return ValidatorResult.Error("PaymentAmount", $"Payment amount must be between {MinPaymentAmount.ToString("C0")} and {MaxPaymentAmount.ToString("C0")}");
            }

            return ValidatorResult.Ok();
        }
    }
}