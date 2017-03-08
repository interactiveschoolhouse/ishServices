using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class RegistrationConfig
    {
        public RegistrationConfig(decimal minPaymentAmount, decimal maxPaymentAmount)
        {
            MinPaymentAmount = minPaymentAmount;
            MaxPaymentAmount = maxPaymentAmount;
        }

        public decimal MinPaymentAmount { get; private set; }
        public decimal MaxPaymentAmount { get; private set; }


    }
}