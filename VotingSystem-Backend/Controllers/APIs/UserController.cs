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
        public IUser? GetSignIn(string username, string password)
        {
            IUser? user = UserManager.GetUser(username, password);
            if (user == null)
            {
                Console.WriteLine(@$"User w/ u:'{username}' p:'{password}' does not exist");
                return new Voter();
            }
            Console.WriteLine(@$"Signing in user s:'{user.SerialNumber}' w/ u:'{username}' p:'{password}'");
            return user;
        }

        [HttpGet]
        [Route("api/sign-in-email")]
        public IUser? GetSignInEmail(string email, string password)
        {
            IUser? user = UserManager.GetUser(email, password);
            if (user == null)
            {
                Console.WriteLine(@$"User w/ e:'{email}' p:'{password}' does not exist");
                return new Voter();
            }
            Console.WriteLine(@$"Signing in user s:'{user.SerialNumber}' w/ u:'{email}' p:'{password}'");
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
