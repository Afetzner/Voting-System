using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace VotingSystem.Controller
{
    class DbConnecter
    {

        private static readonly MySqlConnectionStringBuilder ConnstrBuilder = new ()
        {
            Server = "cse.unl.edu",
            UserID = "afetzner",
            Password = "A4WX9RSt", //I don't use this for anything else, btw...
            Port = 3306
        };

        public static readonly string ConnectionString = ConnstrBuilder.ConnectionString;

        public static bool TestConnection(bool logToConsole = false)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException)
                {
                    if (logToConsole)
                        Console.WriteLine("Failure to connect");
                    return false;
                }
                if (logToConsole)
                    Console.WriteLine("Connection success");
                conn.Close();
                return true;
            }
        }

    }
}