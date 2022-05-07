using MySql.Data.MySqlClient;
using VotingSystem.Model;
using VotingSystem.Utils;

namespace VotingSystem.Accessor {
    /// <summary>
    /// Utility methods for reseting DB and loading example data 
    /// </summary>
    public class DbInitializer
    {
        private static DirectoryInfo? homeDir = TryGetSolutionDirectoryInfo();
        
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

        //Very slow, don't use in productuon code, only for testing
        public static void LoadDummyDataGenerated(int numOfVoters)
        {
            Console.WriteLine("Reseting DB");
            ResetDbTables();

            Console.WriteLine("Creating admins");
            //Add admins
            Admin alexf = new Admin.Builder()
                .WithFirstName("Alex")
                .WithLastName("Fetzner")
                .WithUsername("anfet00")
                .WithEmail("anfet@email.com")
                .WithPassword("Drowsap1!")
                .WithSerialNumber(Admin.Accessor.GetSerial())
                .Build();
            Admin alexb = new Admin.Builder()
                .WithFirstName("Alex")
                .WithLastName("Busch")
                .WithUsername("abusch8")
                .WithEmail("abusch8@huskers.unl.edu")
                .WithPassword("TheBigS0up&")
                .WithSerialNumber(Admin.Accessor.GetSerial())
                .Build();
            Admin ana = new Admin.Builder()
                .WithFirstName("Anastasiya")
                .WithLastName("Krestovsky")
                .WithUsername("3akycka")
                .WithEmail("AKrestovsjy@email.com")
                .WithPassword("pa55W0RD@")
                .WithSerialNumber(Admin.Accessor.GetSerial())
                .Build();
            Admin khonda = new Admin.Builder()
                .WithFirstName("Khondamirkhon")
                .WithLastName("Umarov")
                .WithUsername("Khonda")
                .WithEmail("KhonRov@email.com")
                .WithPassword("H3adOfCh335e?")
                .WithSerialNumber(Admin.Accessor.GetSerial())
                .Build();
            Admin janice = new Admin.Builder()
                .WithFirstName("Janice")
                .WithLastName("Chng")
                .WithUsername("jslc1997")
                .WithEmail("janice@email.com")
                .WithPassword("Th!sIsN0tAPa55w0rd")
                .WithSerialNumber(Admin.Accessor.GetSerial())
                .Build();

            Console.WriteLine("Adding admins");
            Admin.Accessor.AddUser(alexf);
            Admin.Accessor.AddUser(alexb);
            Admin.Accessor.AddUser(ana);
            Admin.Accessor.AddUser(khonda);
            Admin.Accessor.AddUser(janice);

            //Create Issues
            Console.WriteLine("Creating issues");
            BallotIssue issue1 = new BallotIssue.Builder()
                .WithSerialNumber(BallotIssue.Accessor.GetSerial())
                .WithTitle("Mayor of Lincoln")
                .WithStartDate(DateTime.Now)
                .WithEndDate(DateTime.Now.AddHours(1))
                .WithOptions("Michie Bull", "Emma Stone", "Edward W.R. White")
                .Build();
            BallotIssue issue2 = new BallotIssue.Builder()
                .WithSerialNumber(BallotIssue.Accessor.GetSerial())
                .WithTitle("Lincoln City Police Chief")
                .WithStartDate(DateTime.Now)
                .WithEndDate(DateTime.Now.AddHours(1))
                .WithOptions("Ander Roman", "Mich Micheal")
                .Build();
            BallotIssue issue3 = new BallotIssue.Builder()
                .WithSerialNumber(BallotIssue.Accessor.GetSerial())
                .WithTitle("Lincoln City Treasurer")
                .WithStartDate(DateTime.Now)
                .WithEndDate(DateTime.Now.AddHours(1))
                .WithOptions("Cindy Lahof")
                .Build();
            BallotIssue issue4 = new BallotIssue.Builder()
                .WithSerialNumber(BallotIssue.Accessor.GetSerial())
                .WithTitle("Nebraska State Representative")
                .WithStartDate(DateTime.Now)
                .WithEndDate(DateTime.Now.AddDays(1))
                .WithOptions("Sue Han", "Racheal Steep", "Jose Huervo", "Keith Travis")
                .Build();
            BallotIssue issue5 = new BallotIssue.Builder()
                .WithSerialNumber(BallotIssue.Accessor.GetSerial())
                .WithTitle("Nebraska State Governor")
                .WithStartDate(DateTime.Now)
                .WithEndDate(DateTime.Now.AddDays(1))
                .WithOptions("Douglas Delfino", "Judith Mia")
                .Build();
            BallotIssue issue6 = new BallotIssue.Builder()
                .WithSerialNumber(BallotIssue.Accessor.GetSerial())
                .WithTitle("Referandum: Should a new elementary school be built?")
                .WithStartDate(DateTime.Now)
                .WithEndDate(DateTime.Now.AddDays(1))
                .WithOptions("Yes", "No")
                .Build();

            var issueLst = new List<BallotIssue>() 
                { issue1, issue2, issue3, issue4, issue5, issue6};

            foreach(var issue in issueLst)
            {
                Console.WriteLine($@"Adding issue {issue.Title}");
                BallotIssue.Accessor.AddIssue(issue);
            }

            Random rng = new();
            //Generate voters w/ ballots
            Console.WriteLine("Generating voteres & their ballots");
            for (int i = 0; i < numOfVoters; i++)
            {
                int randNum = rng.Next(1000000000, int.MaxValue);
                string first = GenerateName();
                string last = GenerateName();
                int t = last.Length < 4 ? last.Length : 4;
                string username = string.Concat(first, last[..t], randNum.ToString().Substring(3, 7));
                string pass = GeneratePassword();
                string email = string.Concat(first[0..1] + last + randNum.ToString()[..2] + "@email.com");
                Voter voter = new Voter.Builder()
                    .WithSerialNumber(Voter.Accessor.GetSerial())
                    .WithFirstName(first)
                    .WithLastName(last)
                    .WithUsername(username)
                    .WithPassword(pass)
                    .WithEmail(email)
                    .Build();
                Console.WriteLine($@"Voter {i}: {first} {last}");
                Voter.Accessor.AddUser(voter);

                //Add ballots
                foreach(BallotIssue issue in issueLst)
                {
                    //Select whether voter votes on this issue or not
                    if (((randNum >>=1) & 1 ) == 0)
                        continue;

                    randNum >>= 1;
                    Ballot ballot = new Ballot.Builder()
                        .WithIssue(issue.SerialNumber)
                        .WithVoter(voter.SerialNumber)
                        .WithChoice(randNum % issue.Options.Count)
                        .WithSerialNumber(Ballot.Accessor.GetSerial())
                        .Build();
                    Console.WriteLine(@$"    ballot for issue {issue.Title} choice {ballot.Choice}");
                    Ballot.Accessor.AddBallot(ballot);
                }
            }
        }

        //Retrives a name from SQL/Data/names.txt -- graciously provided by https://www.usna.edu/Users/cs/roche/courses/s15si335/proj1/files.php%3Ff=names.txt.html
        private static string GenerateName()
        {
            if (homeDir == null)
                throw new Exception("Could not find solution home directory");
            var path = Path.Combine(homeDir.FullName, "VotingSystem-Backend/SQL/Data/names.txt");
            
            string name = "";
            do
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    name = RandomLine(sr);
                }
            } while (!Validation.IsValidName(name));
            return name;
        }

        //Gotten from https://stackoverflow.com/questions/3745934/read-random-line-from-a-file-c-sharp
        private static string RandomLine(StreamReader reader)
        {
            string chosen = "errName";
            string? line;
            int numberSeen = 0;
            var rng = new Random();
            while ((line = reader.ReadLine()) != null)
            {
                if (rng.Next(++numberSeen) == 0)
                {
                    chosen = line;
                }
            }
            return chosen;
        }

        private static string GeneratePassword()
        {
            Random rng = new Random();
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string nums = "0123456789";
            string specials = "!@#$%&?_";
            string pass;
            do
            {
                pass = "";
                int len = rng.Next(6, 31);
                for (int i =0; i < len; i++)
                {
                    int rand = rng.Next();
                    int which = rand % 4;
                    rand >>= 4;
                    switch (which)
                    {
                        case 0:
                            pass += uppers[rand % uppers.Length];
                            break;
                        case 1:
                            pass += lowers[rand % lowers.Length];
                            break;
                        case 2:
                            pass += nums[rand % nums.Length];
                            break;
                        case 3:
                            pass += specials[rand % specials.Length];
                            break;
                    }
                }

            } while (!Validation.IsValidPassword(pass));
            return pass;
        }
    }
}
