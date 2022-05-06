using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    // ======================
    // ====== UNTESTED ======
    // ======================

    [ApiController]
    public class ResultController : ControllerBase
    {
        private static readonly ResultCacheManager cache = ResultCacheManager.SharedCacheManager;

        [HttpGet]
        [Route("api/issueResults")]
        public List<int> GetIssueResults(string issueSerial)
        {
            //issueSerial does not match any cached issues
            if (!cache.GetIssues().ContainsKey(issueSerial))
            {
                Console.WriteLine(@$"Warning: get issues results for issue not in cache {issueSerial}");
                return new List<int>();
            }
            //issueSerial not gotten in cache => Something terrible happend
            if (!cache.GetResults().ContainsKey(issueSerial))
            {
                Console.WriteLine($@"Critical Warning! get issues results results not found in cache");
                return new List<int>();
            }
            var issue = cache.GetIssues()[issueSerial];
            var results = cache.GetResults()[issueSerial];
            List<int> lst = new();
            //Enforce order of results to be same as options
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
            Dictionary<string, Ballot?> ballots = cache.GetBallots(voterSerial);
            //Issue doesn't exist in cache 
            if (!ballots.ContainsKey(issueSerial))
            {
                Console.WriteLine($@"Warning: Get ballot, issue not contained in cache {issueSerial}");
                return -102;
            }   
            // Voter didn't vote on that issue
            if (ballots[issueSerial] == null) 
                return -1;

            //Can't possibly be null
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return ballots[issueSerial].Choice;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [HttpGet]
        [Route("api/voterParticipation")]
        public List<Voter> GetVoterParticipation(string issueSerial)
        {
            //Issue does not match any in cache
            if (!cache.GetIssues().ContainsKey(issueSerial))
            {
                Console.WriteLine($@"Warning: Get voter participation for issue not in cache {issueSerial}");
                return new List<Voter>();
            }

            Dictionary<string, List<Voter>> paritcipation = cache.GetVoterParticipation();
            //Issue not retireived => Somthing very bad happened
            if (!paritcipation.ContainsKey(issueSerial))
            {
                Console.WriteLine($@"Critical Warning! Get voter participation not retrived from cache {issueSerial}");
                return new List<Voter>();
            }
            return paritcipation[issueSerial];
        }

        [HttpGet]
        [Route("api/voterIssueParticipation")]
        public bool GetDidVoterParticipate(string issueSerial, string voterSerial)
        {
            //Issue does not match any in cache
            if (!cache.GetIssues().ContainsKey(issueSerial))
            {
                Console.WriteLine($@"Warning: Get did voter participate for issue not in cache {issueSerial}");
                return false;
            }

            var dict = cache.GetVoterParticipation();
            //Issue not retirved with cache => somthing very bad happened
            if (!dict.ContainsKey(issueSerial))
            {
                Console.WriteLine($@"Critical Warning! Get did voter participate, issue not retirved cache {issueSerial}");
                return false;
            }
                
            return dict[issueSerial].Exists(x => x.SerialNumber == voterSerial);
        }
    }
}
