using Microsoft.AspNetCore.Mvc;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class ResultViewerController : ControllerBase
    {
        Manager manager = Manager.manager;
 

        [HttpPost]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {
            return manager.GetBallotIssues();
        }

        [HttpPost]
        [Route("api/ballots")]
        public Dictionary<string, Ballot?> GetBallots(string voterSerial) {
            
            return manager.GetBallots(voterSerial);
        }

        [HttpPost]
        [Route("api/results")]
        public Dictionary<string, List<Voter>> GetVoterParticipation()
        {
                return manager.GetVoterParticipation();
        }
    }
}
