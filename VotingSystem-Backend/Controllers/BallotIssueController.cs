using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class BallotIssueController : ControllerBase
    {        
        [HttpGet]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {
            return BallotIssue.Accessor.GetBallotIssues(); 
        }
    }
}