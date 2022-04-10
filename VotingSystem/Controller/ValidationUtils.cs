using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace VotingSystem.Controller
{
    public class ValidationUtils
    {
        /// <summary>Is an alphabetic string of less than 32 chars. No whitespace. Case insensitive </summary>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            Regex rx = new Regex(@"^[a-z]{2,32}$", RegexOptions.IgnoreCase);
            return rx.IsMatch(name);
        }

        /// <summary>Is a capital letter followed by 9 digits</summary>
        public static bool IsValidLicense(string license)
        {
            if (string.IsNullOrWhiteSpace(license))
                return false;
            Regex rx = new Regex("^[A-Z][0-9]{8}$");
            return rx.IsMatch(license);
        }

        ///<summary>Is 6-32 characters, alphanumeric </summary>
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            Regex rx = new Regex("^([a-zA-Z0-9]){6,32}$");
            return rx.IsMatch(username);
        }

        ///<summary>Is 6-32 characters, at least one lower case, upper case, number & special character</summary>
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            Regex acceptedChars = new Regex("^([a-zA-Z0-9!@#$%&?_]){6,32}$");
            Regex hasUpper = new Regex("[A-Z]+");
            Regex hasLower = new Regex("[a-z]+");
            Regex hasNumber = new Regex("[0-9]+");
            Regex hasSpecial = new Regex("[!@#$%&?_]+");

            return acceptedChars.IsMatch(password) & 
                   hasUpper.IsMatch(password) &
                   hasLower.IsMatch(password) & 
                   hasNumber.IsMatch(password) &
                   hasSpecial.IsMatch(password);
        }
    }
}
