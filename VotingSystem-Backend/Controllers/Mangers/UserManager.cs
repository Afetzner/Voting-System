using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class UserEngine : ControllerBase
    {
        private static readonly ResultCacheEngine cache = ResultCacheEngine.SharedCacheManager;

        [HttpGet]
        [Route("api/sign-in")]
        public IUser? GetSignIn(string usernameSlashEmail, string password)
        {
            IUser? user = UserEngine.GetUser(usernameSlashEmail, password);
            if (user == null)
            {
                Console.WriteLine(@$"User w/ u:'{usernameSlashEmail}' p:'{password}' does not exist");
                return new Voter();
            }
            Console.WriteLine(@$"Signing in user s:'{user.SerialNumber}' w/ u:'{usernameSlashEmail}' p:'{password}'");
            return user;
        }

        [HttpGet]
        [Route("api/vote")]
        public bool PostVote(string userSerialNumber, string issueSerialNumber, int choice)
        {
             Ballot ballot = new Ballot.Builder()
                        .WithIssue(issueSerialNumber)
                        .WithVoter(userSerialNumber)
                        .WithChoice(choice)
                        .WithSerialNumber(Ballot.Accessor.GetSerial())
                        .Build();

            return BallotEngine.AddBallot(ballot);
        }

        [HttpPost]
        [Route("api/voted")]
        public BallotIssueOption PostVoted(string userSerialNumber, string balletSerialNumber)
        {
            return new BallotIssueOption(0, "UNKNOWN");
        }
    }
}
