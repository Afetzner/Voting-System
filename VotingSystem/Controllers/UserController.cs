using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controller
{
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("api/user")]
        public User GetUser()
        {
            return new("abusch8", "1234", "abusch8@huskers.unl.edu", "Alex", "Busch", false); 
        }
    }
}
