using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controller
{

    [ApiController]
    public class UserController : ControllerBase
    {
        private User test = new User("abusch8", "1234", "abusch8@huskers.unl.edu", "Alex", "Busch", false);

        [HttpGet]
        [Route("api/user")]
        public User GetUser()
        {
            return test; 
        }
    }
}
