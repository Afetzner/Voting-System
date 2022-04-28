using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace VotingSystem.Controller {
    public class DbInitializer
    {
        //From stack overflow: https://stackoverflow.com/questions/19001423/getting-path-to-the-parent-folder-of-the-solution-file-using-c-sharp

        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
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
            DirectoryInfo homeDir = TryGetSolutionDirectoryInfo();
            if (homeDir == null)
                throw new Exception("Cannot find solution home directory");
            var queryFile = Path.Combine(homeDir.FullName, "VotingSystem/SQL/Scripts/ResetDb.sql");
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

        public static void LoadDummyData()
        {
            DirectoryInfo homeDir = TryGetSolutionDirectoryInfo();
            if (homeDir == null)
                throw new Exception("Cannot find solution home directory");
            var queryFile = Path.Combine(homeDir.FullName, "VotingSystem/SQL/Scripts/LoadTestData.sql");
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
    }
}
