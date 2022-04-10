﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Model
{
    class Admin : IUser
    {
        public string Username { get; }
        public string Password { get; }

        public Admin(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}