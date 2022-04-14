using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

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

        ///<summary> Is 2 Capital characters, must be American State.
        /// copied from: https://stackoverflow.com/questions/176106/validate-string-against-usps-state-abbreviations
        private static String states = "|AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY|";

        public static bool IsValidState (string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                return false;
            return state.Length == 2 && states.IndexOf( state ) > 0;
        }

        public static bool IsValidDistrict(int district)
        {
            if (district < 0 || district > 100)
                return false;
            return true;
        }

        public static bool IsValidDate(string date)
        {
            if (String.IsNullOrWhiteSpace(date))
                return false;
            
            Regex rx = new Regex("^2[0-9]{3}-(0[0-9])|(1[012])-[0123][0-9]$");
            if (!rx.IsMatch(date))
            {
                return false;
            }

            Dictionary<string, int> daysInMonth = new Dictionary<string, int>
            {
                ["00"] = -1, //I don't know why it was matching xxxx-00-xx, but this fixed it
                ["01"] = 31,
                ["02"] = 28,
                ["03"] = 31,
                ["04"] = 30,
                ["05"] = 31,
                ["06"] = 30,
                ["07"] = 31,
                ["08"] = 31,
                ["09"] = 30,
                ["10"] = 31,
                ["11"] = 30,
                ["12"] = 31
            };

            string year = date.Substring(0, 4);
            string month = date.Substring(5, 2);
            string day = date.Substring(8, 2);

            //Feb 29 leap years
            if (int.Parse(year) % 4 == 0 &
                month == "02" &
                day == "29")
                return true;

            return (daysInMonth.ContainsKey(month) &
                int.Parse(day) != 0 &
                int.Parse(day) <= daysInMonth[month]);
        }
    }
}
