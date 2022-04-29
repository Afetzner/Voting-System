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
    interface IDbBallotIssueAccessor
    {
        bool AddIssue(BallotIssue issue);

        void RemoveIssue(string serial);

        List<BallotIssue> GetBallotIssues();

    }
}
