using Microsoft.AspNetCore.Mvc;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly ResultCacheManager cache = ResultCacheManager.SharedCacheManager;

        [HttpGet]
        [Route("api/sign-in")]
        public IUser? GetSignIn(string usernameSlashEmail, string password)
        {
            IUser? user = UserManager.GetUser(usernameSlashEmail, password);
            if (user == null)
            {
                Console.WriteLine(@$"User w/ u:'{usernameSlashEmail}' p:'{password}' does not exist");
                return new Voter();
            }
            Console.WriteLine(@$"Signing in user s:'{user.SerialNumber}' w/ u:'{usernameSlashEmail}' p:'{password}'");
            return user;
        }
                
        [HttpPost]
        [Route("api/vote")]
        public bool PostVote(string userSerialNumber, string issueSerialNumber, int count, string selection)
        {
            Console.WriteLine(userSerialNumber, issueSerialNumber, new BallotIssueOption(count, selection));
            return true;
        }

        [HttpPost]
        [Route("api/voted")]
        public BallotIssueOption PostVoted(string userSerialNumber, string balletSerialNumber)
        {
            return new BallotIssueOption(0, "UNKNOWN");
        }
    }
}
