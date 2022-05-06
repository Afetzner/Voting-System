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
<<<<<<< HEAD
                
        [HttpPost]
=======

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

        [HttpGet]
>>>>>>> 609af9248e1aebb5f63be7f754cf2c3de733a159
        [Route("api/vote")]
        public bool PostVote(string userSerialNumber, string issueSerialNumber, int choice)
        {
            
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
