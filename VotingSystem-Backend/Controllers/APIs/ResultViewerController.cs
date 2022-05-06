using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    // ======================
    // ====== UNTESTED ======
    // ======================

    [ApiController]
    public class ResultViewerController : ControllerBase
    {
        private static readonly ResultCacheManager cache = ResultCacheManager.SharedCacheManager;

        [HttpGet]
        [Route("api/issueResults")]
        public List<int> GetIssueResults(string issueSerial)
        {
            // Results returned must be in same order as issue options : hence terrible implimentation
            // Could be cleaned up
            BallotIssue? issue = cache.GetIssues().Find(x => x.SerialNumber == issueSerial);
            if (issue == null)
                return new List<int>();

            var results = cache.GetResults()[issueSerial];
            List<int> lst = new();
            foreach (var option in issue.Options)
            {
                lst.Add(results[option.Number]);
            }
            return lst;
        }

        [HttpGet]
        [Route("api/voterIssueBallot")]
        public int GetBallot(string voterSerial, string issueSerial)
        {
            var dict = cache.GetBallots(voterSerial);
            //Somthing terrible happend
            if (dict == null) 
                return -103;
            //Issue doesn't exist
            if (!dict.ContainsKey(issueSerial)) 
                return -102;
            // Voter didn't vote on that issue
            if (dict[issueSerial] == null) 
                return -1;

            //Can't possibly be null
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return dict[issueSerial].Choice;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [HttpGet]
        [Route("api/voterParticipation")]
        public List<Voter> GetVoterParticipation(string issueSerial)
        {
            var dict = cache.GetVoterParticipation();
            if (dict == null)
                return new List<Voter>();
            return dict[issueSerial];
        }

        [HttpGet]
        [Route("api/voterIssueParticipation")]
        public bool GetDidVoterParticipate(string issueSerial, string voterSerial)
        {
            var dict = cache.GetVoterParticipation();
            if (dict == null)
                return false;
            return dict[issueSerial].Exists(x => x.SerialNumber == voterSerial);
        }
    }
}
