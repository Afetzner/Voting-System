using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Model;

namespace VotingSystem.Accessor
{
    //Janice tried writing the ballot-issue-option controller, the problem was that the options 
    // are contained in a list in the issue, so they don't have access to the issue serial num
    // I suggest we move the add_issue_options into here (so issues and their options get added together)
    // We could have a separate func that adds options to existing issue too, but that's low priority.
    public interface IBallotIssueAccessor : IWithSerialNumber
    {
        /// <summary>
        /// Adds an issue and its options to the DB
        /// </summary>
        /// <param name="issue">Issue to be added</param>
        /// <returns>false on serial number/title collision</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool AddIssue(BallotIssue issue);

        /// <summary>
        /// Removes an issue and its options from the DB
        /// </summary>
        /// <param name="serial">Serial number of issue</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public void RemoveIssue(string serial);

        /// <summary>
        /// Gets all the ballot issue (and their options) from the DB
        /// </summary>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        /// <exception cref="InvalidBuilderParameterException">Corrupt data from DB</exception>
        List<BallotIssue> GetBallotIssues();

    }
}
