using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class ResultViewerController : ControllerBase
    {
        private static readonly ResultCacheManager cache = ResultCacheManager.SharedCacheManager;

        [HttpPost]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {
            return cache.GetBallotIssues();
        }

        [HttpPost]
        [Route("api/ballots")]
        public Dictionary<string, Ballot?> GetBallots(string voterSerial) {
            
            return cache.GetBallots(voterSerial);
        }

        [HttpPost]
        [Route("api/results")]
        public Dictionary<string, List<Voter>> GetVoterParticipation()
        {
            return cache.GetVoterParticipation();
        }
    }
}
