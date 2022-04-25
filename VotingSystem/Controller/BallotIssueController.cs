using System;
using System.Collections.Generic;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    class BallotIssueController : IDbBallotIssueController
    {
        //TODO
        public bool AddIssue(BallotIssue issue)
        {
            //Using conn = ...
            //  collision = false;
            //  Using cmd = "add_ballot_issue"
            //  cmd.type = stored procedure
            //  set cmd args to issue.serialNumber, issue.title, ... ect.
            //  execute cmd
            //
            //  foreach (var option in issue.options) 
            //      using cmd = "add_ballot_issue_option"
            //      cmd.type = stored procedure
            //      set cmd args to **serial = issue.seriaNumber**, title = option.title, number = option.number... ect.
            //      Execute cmd
            //      if collision
            //          handle collision, probably just stop and return false?
            //  return collision;
            throw new NotImplementedException();
        }

        //TODO
        public void RemoveIssue(string serial)
        {
            //Look at the VoterController removeUser method for reference, very similar
            throw new NotImplementedException();
        }

        //TODO
        public List<BallotIssue> GetBallotIssues()
        {
            //Using conn = ...
            //  Using cmd = "get_ballot_issue"
            //  cmd.type = stored procedure
            //  set up output cmd args (see voterController getUser)
            //  execute cmd
            //
            //  iterate through returned values
            //  (the return values will be a table, not a single item,
            //  you'll have to figure out how to do this, no other funcs have done this yet)
            //  start building issue with serial number, title ... ect.
            //  foreach returned value {
            //    using cmd = "get_issue_options"
            //    cmd.args["serialNumber"] = current issue being built's serial number
            //    ...
            //    execute cmd
            //    foreach returned value {
            //      var option = ballotIssueOptionBuilder().WithTitle(...)... .Build()
            //      add option to issue
            //   build issue
            //return issues
            throw new NotImplementedException();
        }
    }
}
