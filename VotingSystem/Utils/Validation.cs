using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.String;

namespace VotingSystem.Utils
{
    public class Validation
    {
        /// <summary>Is an alphabetic string of fewer than 32 chars. No whitespace. Case insensitive </summary>
        public static bool IsValidName(string name)
        {
            if (IsNullOrWhiteSpace(name))
                return false;
            var rx = new Regex(@"^[a-z]{2,32}$", RegexOptions.IgnoreCase);
            return rx.IsMatch(name);
        }

        /// <summary>Is a capital letter followed by 8 digits</summary>
        public static bool IsValidSerialNumber(string serialNum)
        {
            if (IsNullOrWhiteSpace(serialNum))
                return false;
            var rx = new Regex("^[A-Z][0-9]{8}$");
            return rx.IsMatch(serialNum);
        }

        ///<summary>Is 6-32 characters, alphanumeric </summary>
        public static bool IsValidUsername(string username)
        {
            if (IsNullOrWhiteSpace(username))
                return false;
            var rx = new Regex("^([a-zA-Z0-9]){6,32}$");
            return rx.IsMatch(username);
        }

        ///<summary>Is 6-32 characters, at least one lower case, upper case, number & special character</summary>
        public static bool IsValidPassword(string password)
        {
            if (IsNullOrWhiteSpace(password))
                return false;

            var acceptedChars = new Regex("^([a-zA-Z0-9!@#$%&?_]){6,32}$");
            var hasUpper = new Regex("[A-Z]+");
            var hasLower = new Regex("[a-z]+");
            var hasNumber = new Regex("[0-9]+");
            var hasSpecial = new Regex("[!@#$%&?_]+");

            return acceptedChars.IsMatch(password) & 
                   hasUpper.IsMatch(password) &
                   hasLower.IsMatch(password) & 
                   hasNumber.IsMatch(password) &
                   hasSpecial.IsMatch(password);
        }

        /// <summary>Date string formatted 'YYYY-MM-DD'</summary>
        public static bool IsValidDate(string date)
        {
            if (IsNullOrWhiteSpace(date))
                return false;
            
            var rx = new Regex("^2[0-9]{3}-(0[0-9])|(1[012])-[0123][0-9]$");
            if (!rx.IsMatch(date))
            {
                return false;
            }

            var daysInMonth = new Dictionary<string, int>
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

            var year = date.Substring(0, 4);
            var month = date.Substring(5, 2);
            var day = date.Substring(8, 2);

            //Feb 29 leap years
            if (int.Parse(year) % 4 == 0 &
                month == "02" &
                day == "29")
                return true;

            return daysInMonth.ContainsKey(month) &
                int.Parse(day) != 0 &
                int.Parse(day) <= daysInMonth[month];
        }
    }
}
