using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class BallotController : ControllerBase
    {
        private static readonly ResultCacheManager cache = ResultCacheManager.SharedCacheManager;

        [HttpPost]
        [Route("api/ballot")]
        public Ballot PostBallot(string issueSerial, string voterSerial, int choice)
        {
             Ballot ballot = new Ballot.Builder()
                        .WithIssue(issueSerial)
                        .WithVoter(voterSerial)
                        .WithChoice(choice)
                        .WithSerialNumber(Ballot.Accessor.GetSerial())
                        .Build();
                    Ballot.Accessor.AddBallot(ballot);
/*
            BallotManager.AddBallot(ballot); //not sure if use this or the one above
*/
            return ballot;
        }
    }
}
