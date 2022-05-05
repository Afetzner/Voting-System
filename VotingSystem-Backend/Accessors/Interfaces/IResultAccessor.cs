using VotingSystem.Model;
using MySql.Data.MySqlClient;
using VotingSystem.Utils;

namespace VotingSystem.Accessor
{

    public interface IResultAccessor
    {
        /// <summary>
        /// Given a issue's serial number, retrives a list of all voters who particpate on that issue
        /// </summary>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        /// <exception cref="InvalidBuilderParameterException">Corrupt data from DB</exception>
        public List<Voter> GetVoterParticipation(string issueSerial);

        /// <summary>
        /// Gets the vote counts for each option for a particular issue
        /// </summary>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public Dictionary<int, int> GetIssueResults(string issueSerial);

        /// <summary>
        /// Gets the vote counts for each option for each issue
        /// </summary>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public Dictionary<string,Dictionary<int, int>> GetAllResults(List<BallotIssue> issues);
    }
}

