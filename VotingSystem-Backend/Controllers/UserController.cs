using Microsoft.AspNetCore.Mvc;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("api/user")]
        public IUser GetUser()
        {
            IUser user = new Voter.VoterBuilder()
                .WithSerialNumber("U12345678")
                .WithUsername("abusch8")
                .WithPassword("a34V&3d")
                .WithEmail("abusch8@huskers.unl.edu")
                .WithLastName("Busch")
                .WithFirstName("Alex")
                .Build();
            return user; 
        }

        [HttpPost]
        [Route("api/sign-in")]
        public bool PostUser(string username, string password)
        {
            Console.WriteLine(username, password);
            return true;
        }

        [HttpPost]
        [Route("api/vote")]
        public bool PostVote(string userSerialNumber, string balletSerialNumber, int count, string selection)
        {
            Console.WriteLine(userSerialNumber, balletSerialNumber, new BallotIssueOption(count, selection));
            return true;
        }
    }
}
