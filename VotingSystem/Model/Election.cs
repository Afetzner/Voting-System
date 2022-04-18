using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem;
using VotingSystem.Controller;

namespace VotingSystem.Model
{
    public class Election
    {
        public string State { get; }
        public int District { get; }
        public string StartDate { get; }
        public string EndDate { get; }
        public List<Candidate> Candidates { get; }

        public Election(string state, int district, string startDate, string endDate)
        {
            State = state;
            District = district;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Election(string state, int district, string startDate, string endDate, List<Candidate> candidates)
        {
            State = state;
            District = district;
            StartDate = startDate;
            EndDate = endDate;
            Candidates = candidates;
        }
    }

    //Grabbed from microsoft documentation https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions 
    [Serializable]
    public class InvalidElectionParameterException : Exception
    {
        public InvalidElectionParameterException() : base() { }
        public InvalidElectionParameterException(string message) : base(message) { }
        public InvalidElectionParameterException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InvalidElectionParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    public class ElectionBuilder
    {
        public string State = null;
        public int District = -1;
        public string StartDate = null;
        public string EndDate = null;
        public List<Candidate> Candidates = new List<Candidate>();

        public ElectionBuilder WithState(string state)
        {
            State = state;
            return this;
        }

        public ElectionBuilder WithDistrict(int district)
        {
            District = district;
            return this;
        }

        public ElectionBuilder WithStartDate(string startDate)
        {
            StartDate = startDate;
            return this;
        }

        public ElectionBuilder WithEndDate(string endDate)
        {
            EndDate = endDate;
            return this;
        }

        public ElectionBuilder WithCandidates(List<Candidate> candidates)
        {
            Candidates = candidates;
            return this;
        }

        public Election Build()
        {
            if (!ValidationUtils.IsValidState(State))
                throw new InvalidElectionParameterException("Invalid state '" + State + "'");
            if (!ValidationUtils.IsValidDistrict(District))
                throw new InvalidElectionParameterException("Invalid district '" + District + "'");
            if (!ValidationUtils.IsValidDate(StartDate))
               throw new InvalidElectionParameterException("Invalid date '" + StartDate + "'");
            if (!ValidationUtils.IsValidDate(EndDate))
                throw new InvalidElectionParameterException("Invalid date '" + EndDate + "'");

            Election election = new Election(State, District, StartDate, EndDate, Candidates);
            return election;
        }
    }
}
