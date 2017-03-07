using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IshServices.Validators
{
    public class EmailValidation
    {
        public static bool IsValid(string email)
        {
            // Return true if strIn is in valid e-mail format.
            //https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx
            try
            {
                if (!Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                {
                    return false;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            return true;
        }
    }
}