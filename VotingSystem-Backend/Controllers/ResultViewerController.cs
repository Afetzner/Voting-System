using Microsoft.AspNetCore.Mvc;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class ResultViewerController : ControllerBase
    {
        ResultViewer resultViewer = SharedResultViewer.ResultViewer;
 

        [HttpPost]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {
            return resultViewer.GetBallotIssues();
        }

        [HttpPost]
        [Route("api/ballots")]
        public Dictionary<string, Ballot?> GetBallots(string voterSerial) {
            
            return resultViewer.GetBallots(voterSerial);
        }

        [HttpPost]
        [Route("api/results")]
        public Dictionary<string, List<Voter>> GetVoterParticipation()
        {
                return resultViewer.GetVoterParticipation();
        }
    }
}
