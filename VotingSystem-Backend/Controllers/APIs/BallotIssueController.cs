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
        public Dictionary<string, BallotIssue> GetBallotIssues()
        {   
            return cache.GetIssues();
        }
    }
}