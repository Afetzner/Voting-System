using MySql.Data.MySqlClient;
using VotingSystem.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VotingSystem.Accessor {
    public class DbInitializer
    {
        //From stack overflow: https://stackoverflow.com/questions/19001423/getting-path-to-the-parent-folder-of-the-solution-file-using-c-sharp
        private static DirectoryInfo? TryGetSolutionDirectoryInfo(string? currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
        
        public static void ResetDbTables()
        {
            DirectoryInfo? homeDir = TryGetSolutionDirectoryInfo();
            if (homeDir == null)
                throw new Exception("Cannot find solution home directory");
            var queryFile = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Scripts/ResetDb.sql");
            string sql = File.ReadAllText(queryFile);

            using (MySqlConnection conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException)
                {
                    Console.WriteLine("Could not connect to DB");
                    throw;
                }

                MySqlScript script = new MySqlScript(conn, sql);
                script.Delimiter = "$$";

                try
                {
                    script.Execute();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public static void LoadDummyDataFromSql()
        {
            DirectoryInfo? homeDir = TryGetSolutionDirectoryInfo();
            if (homeDir == null)
                throw new Exception("Cannot find solution home directory");
            var queryFile = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Scripts/LoadTestData.sql");
            string sql = File.ReadAllText(queryFile);

            using (MySqlConnection conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException)
                {
                    Console.WriteLine("Could not connect to DB");
                    throw;
                }

                MySqlScript script = new MySqlScript(conn, sql);
                script.Delimiter = "$$";

                try
                {
                    script.Execute();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public static void LoadDummyDataFromJson()
        {
            DirectoryInfo? homeDir = TryGetSolutionDirectoryInfo();
            if (homeDir == null)
                throw new Exception("Cannot find solution home directory");

            var file = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Data/Voters.json");
            string data = File.ReadAllText(file);
            var voters = JsonSerializer.Deserialize<List<Voter>>(data);

            file = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Data/Admins.json");
            data = File.ReadAllText(file);
            var admins = JsonSerializer.Deserialize<List<Admin>>(data);

            file = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Data/BallotIssues.json");
            data = File.ReadAllText(file);
            var issues = JsonSerializer.Deserialize<List<BallotIssue>>(data);

            file = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Data/Ballots.json");
            data = File.ReadAllText(file);
            var ballots = JsonSerializer.Deserialize<List<Ballot>>(data);

            if (voters == null || admins == null || issues == null || ballots == null)
                throw new NullReferenceException();

            foreach (Admin admin in admins) 
                Admin.Accessor.AddUser(admin);

            foreach (Voter voter in voters) 
                Voter.Accessor.AddUser(voter);

            foreach (BallotIssue issue in issues)
                BallotIssue.Accessor.AddIssue(issue);

            foreach (Ballot ballot in ballots)
                Ballot.Accessor.AddBallot(ballot);
        }
    }
}
