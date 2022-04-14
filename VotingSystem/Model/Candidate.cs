using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;

namespace VotingSystem.Model
{
    public class Candidate
    {
        public string LastName { get; }
        public string FirstName { get; }
        
        public Candidate(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }
    }

    //Grabbed from microsoft documentation https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions 
    [Serializable]
    public class InvalidCandidateParameterException : Exception
    {
        public InvalidCandidateParameterException() : base() { }
        public InvalidCandidateParameterException(string message) : base(message) { }
        public InvalidCandidateParameterException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InvalidCandidateParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class CandidateBuilder
    {
        public string LastName = null;
        public string FirstName = null;

        public CandidateBuilder WithLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }

        public CandidateBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }


        public Candidate Build()
        {
            if (!ValidationUtils.IsValidName(LastName))
                throw new InvalidCandidateParameterException("Invalid last name '" + LastName + "'");
            if (!ValidationUtils.IsValidName(FirstName))
                throw new InvalidCandidateParameterException("Invalid first name '" + FirstName + "'");
          
            Candidate candidate = new Candidate(LastName, FirstName);
            return candidate;
        }
    }
}
