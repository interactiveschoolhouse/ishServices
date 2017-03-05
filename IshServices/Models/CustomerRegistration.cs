﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class CustomerRegistration
    {
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal PaymentAmount { get; set; }
        public string SpecialInstructions { get; set; }
    }
}