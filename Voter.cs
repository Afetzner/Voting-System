using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCE361_voting_system;

namespace CSCE361_voting_system
{
    class Voter : IUser
    {
        private int _voterId { get; }
        private string _lastName { get; }
        private string _firstName { get; }
        private string _middleName { get; }
        private string _licenseNumber { get; }

        public Voter(string lastName, string firstName, string middleName, string licenseNumber)
        {
            _lastName = lastName;
            _firstName = firstName;
            _middleName = middleName;
            _licenseNumber = licenseNumber;
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
    
    class VoterBuilder
    {
        public string lastName = null;
        public string firstName = null;
        public string middleName = null;
        public string licenseNumber = null;

        public VoterBuilder withLastName(string lastName)
        {
            this.lastName = lastName;
            return this;
        }

        public VoterBuilder withFirstName(string firstName)
        {
            this.firstName = firstName;
            return this;
        }

        public VoterBuilder withMiddleName(string middleName)
        {
            this.middleName = middleName;
            return this;
        }

        public VoterBuilder withLicenseNumber(string licenseNumber)
        {
            this.licenseNumber = licenseNumber;
            return this;
        }

        public Voter Build()
        {
            if (lastName == null)
                throw new InvalidBuilderParameterException("Last name cannot be null");
            if (firstName == null)
                throw new InvalidBuilderParameterException("First name cannot be null");
            if (middleName == null)
                throw new InvalidBuilderParameterException("Middle name cannot be null");
            if (licenseNumber == null)
                throw new InvalidBuilderParameterException("License number cannot be null");

            Voter voter = new Voter(lastName, firstName, middleName,licenseNumber);
            return voter;
        }
    }
}
