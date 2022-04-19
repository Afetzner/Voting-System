using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Model
{
    interface IUser
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string Password { get; }
    }
}
