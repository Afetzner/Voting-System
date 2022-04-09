using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CSCE361_voting_system.SQL
{
    class MySqlConnector
    {

        static readonly MySqlConnectionStringBuilder Connstr = new ()
        {
            Server = "cse.unl.edu",
            UserID = "afetzner",
            Password = "A4WX9RSt", //I don't use this for anything else, btw...
            Port = 3306
        };

        public static void TestConnection()
        {
            MySqlConnection conn = new MySqlConnection(Connstr.ConnectionString);
            try
            {
                conn.Open();
                Console.WriteLine("Connected!");
            }
            catch (MySqlException)
            {
                Console.WriteLine("Failure to connect");
                throw;
            }
            finally
            {
                conn.Close();
                Console.WriteLine("Closed");
            }
        }
    }
}