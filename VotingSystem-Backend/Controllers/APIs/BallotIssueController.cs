using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class BallotIssueController : ControllerBase
    {
        private static readonly ResultCacheManager cache = ResultCacheManager.SharedCacheManager;

        [HttpGet]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {   
            return cache.GetBallotIssues();
        }
    }
}