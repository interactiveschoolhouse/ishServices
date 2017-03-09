using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Validators
{
    public class ValidatorResult
    {
        private ValidatorResult()
        {

        }

        private static ValidatorResult _ok = new ValidatorResult() { IsValid = true };

        public static ValidatorResult Ok()
        {
            return _ok;
        }

        public static ValidatorResult Error(string name, string description)
        {
            return new ValidatorResult() { IsValid = false, Name = name, Description = description };
        }


        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsValid { get; private set; }
    }
}