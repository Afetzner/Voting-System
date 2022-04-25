using VotingSystem.Controller;
using VotingSystem.Utils;

namespace VotingSystem.Model
{
    public class Voter : IUser
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string Password { get; }
        public string LastName { get; }
        public string FirstName { get; }

        public static bool IsAdmin = false;

        public static readonly IDbUserController<Voter> DbController = new VoterController();

        public Voter(string username, string password, string lastName, string firstName, string serialNumber)
        {
            SerialNumber = serialNumber;
            Username = username;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
        }
    }

    public class VoterBuilder
    {
        public string SerialNumber;
        public string Username;
        public string Password;
        public string LastName;
        public string FirstName;

        public VoterBuilder WithSerialNumber(string serialNum)
        {
            SerialNumber = serialNum;
            return this;
        }

        public VoterBuilder WithUsername(string username)
        {
            Username = username;
            return this;
        }

        public VoterBuilder WithPassword(string password)
        {
            Password = password;
            return this;
        }

        public VoterBuilder WithLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }

        public VoterBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }

        public Voter Build()
        {
            if (!Validation.IsValidSerialNumber(SerialNumber))
                throw new InvalidBuilderParameterException("Invalid serial number '" + SerialNumber + "'");
            if (!Validation.IsValidUsername(Username))
                throw new InvalidBuilderParameterException("Invalid username '" + Username + "'");
            if (!Validation.IsValidPassword(Password))
                throw new InvalidBuilderParameterException("Invalid password '" + Password + "'");
            if (!Validation.IsValidName(LastName))
                throw new InvalidBuilderParameterException("Invalid last name '" + LastName + "'");
            if (!Validation.IsValidName(FirstName))
                throw new InvalidBuilderParameterException("Invalid first name '" + FirstName + "'");
            
            Voter voter = new (Username, Password, LastName, FirstName,  SerialNumber);
            return voter;
        }
    }
}
