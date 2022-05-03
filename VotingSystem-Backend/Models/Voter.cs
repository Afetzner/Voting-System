using VotingSystem.Utils;
using VotingSystem.Accessor;

namespace VotingSystem.Model
{
    public class Voter : IUser
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public bool IsAdmin { get { return false; } }

        public static readonly IUserAccessor<Voter> Accessor = new UserDbAccessor<Voter>();

        private Voter(string username, string password, string email, string lastName, string firstName, string serialNumber)
        {
            SerialNumber = serialNumber;
            Username = username;
            Password = password;
            Email = email;
            LastName = lastName;
            FirstName = firstName;
        }
        public class Builder
        {
            public string? SerialNumber;
            public string? Username;
            public string? Email;
            public string? Password;
            public string? LastName;
            public string? FirstName;

            public Builder WithSerialNumber(string? serialNum)
            {
                SerialNumber = serialNum;
                return this;
            }

            public Builder WithUsername(string? username)
            {
                Username = username;
                return this;
            }

            public Builder WithPassword(string? password)
            {
                Password = password;
                return this;
            }

            public Builder WithEmail(string? email)
            {
                Email = email;
                return this;
            }

            public Builder WithLastName(string? lastName)
            {
                LastName = lastName;
                return this;
            }

            public Builder WithFirstName(string? firstName)
            {
                FirstName = firstName;
                return this;
            }

            public Voter Build()
            {
                if (SerialNumber == null || !Validation.IsValidSerialNumber(SerialNumber))
                    throw new InvalidBuilderParameterException("Invalid serial number '" + SerialNumber + "'");
                if (Username == null || !Validation.IsValidUsername(Username))
                    throw new InvalidBuilderParameterException("Invalid username '" + Username + "'");
                if (Password == null || !Validation.IsValidPassword(Password))
                    throw new InvalidBuilderParameterException("Invalid password '" + Password + "'");
                if (LastName == null || !Validation.IsValidName(LastName))
                    throw new InvalidBuilderParameterException("Invalid last name '" + LastName + "'");
                if (FirstName == null || !Validation.IsValidName(FirstName))
                    throw new InvalidBuilderParameterException("Invalid first name '" + FirstName + "'");
                if (Email == null)
                    throw new InvalidBuilderParameterException("Invalid email (null)'" + Email + "'");

                Voter voter = new(Username, Password, Email, LastName, FirstName, SerialNumber);
                return voter;
            }
        }
    }
}
