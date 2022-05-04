using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class BallotIssueController : ControllerBase
    {
        //Shared context for all api calls
        private SharedResultViewer resultViewer = ResultsManager.ResultViewer;

        [HttpGet]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {   
            return resultViewer.GetBallotIssues();
        }
    }
}