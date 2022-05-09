using VotingSystem.Model;
using VotingSystem.Utils;
using MySql.Data.MySqlClient;

namespace VotingSystem.Accessor
{
    public interface IBallotAccessor : IWithSerialNumber
    {
        /// <summary>
        /// Adds a ballot to the  DB
        /// </summary>
        /// <param name="ballot">Ballot to be added</param>
        /// <returns>false on serial number/user-voter collision</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool AddBallot(Ballot ballot);

        /// <summary>
        /// Removes a ballot from the DB
        /// </summary>
        /// <param name="serial">Serial number of ballot</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public void RemoveBallot(string serial);

        /// <summary>
        /// Returns a (particular) voter's ballot on a particular issue
        /// </summary>
        /// <returns>The corresponding ballot, null if voter did not vote on that issue</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        /// <exception cref="InvalidBuilderParameterException">Corrupt data from DB</exception>
        Ballot? GetBallot(string voter, string issue);
    }
}