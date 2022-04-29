using Microsoft.AspNetCore.Mvc;
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
            return null; 
        }
    }
}
