using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;

namespace VotingSystem.Model
{
    public class Voter : IUser
    {
        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LicenseNumber { get; }

        public static IDbController<Voter> DbController = new VoterController();

        public Voter(string lastName, string firstName, string middleName, string licenseNumber)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            LicenseNumber = licenseNumber;
        }
    }

    //Grabbed from microsoft documentation https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions 
    [Serializable]
    public class InvalidBuilderParameterException : Exception
    {
        public InvalidBuilderParameterException() : base() { }
        public InvalidBuilderParameterException(string message) : base(message) { }
        public InvalidBuilderParameterException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InvalidBuilderParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    public class VoterBuilder
    {
        public string LastName = null;
        public string FirstName = null;
        public string MiddleName = null;
        public string LicenseNumber = null;

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

        public VoterBuilder WithMiddleName(string middleName)
        {
            MiddleName = middleName;
            return this;
        }

        public VoterBuilder WithLicenseNumber(string licenseNumber)
        {
            LicenseNumber = licenseNumber;
            return this;
        }

        public Voter Build()
        {
            if (!ValidationUtils.IsValidName(LastName))
                throw new InvalidBuilderParameterException("Invalid last name '" + LastName + "'");
            if (!ValidationUtils.IsValidName(FirstName))
                throw new InvalidBuilderParameterException("Invalid first name '" + FirstName + "'");
            if (String.IsNullOrWhiteSpace(MiddleName))
                throw new InvalidBuilderParameterException("Middle name cannot be null or white space");
            if (!ValidationUtils.IsValidLicense(LicenseNumber))
                throw new InvalidBuilderParameterException("Invalid license number '" + LicenseNumber + "'");

            Voter voter = new Voter(LastName, FirstName, MiddleName, LicenseNumber);
            return voter;
        }
    }
}
