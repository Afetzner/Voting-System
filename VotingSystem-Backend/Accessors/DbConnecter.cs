using MySql.Data.MySqlClient;

namespace VotingSystem.Accessor
{
    public class DbConnecter
    {

        private static readonly MySqlConnectionStringBuilder ConnstrBuilder = new()
        {
            Server = "cse.unl.edu",
            Database = "afetzner",
            UserID = "afetzner",
            Password = "A4WX9RSt",
            Port = 3306
        };

        // This is how we *would* make hide our connection string
        // Except it keeps trying to format it like a Microsoft SQL conn-str
        // Absolutly could not figure out how to fix it.
        // TA also couldn't help
        private static readonly string _connectionString = 
            System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;

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