using VotingSystem.Model;
using System.Data;
using MySql.Data.MySqlClient;

namespace VotingSystem.Accessor
{
    public class ResultAccessor : IResultAccessor
    {
        public static readonly IResultAccessor Instance = new ResultAccessor();   

        public List<BallotIssue> GetBallotIssues()
        {
            //List of ballot-issues *builders* (all get built at end)
            var ballotIssueBuilderList = new List<BallotIssue.Builder>();
            var ballotIssueList = new List<BallotIssue>();

            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_issues", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        //Ballot-issue W/OUT OPTIONS
                        // (can't execute the get-options query in the middle of getting issues
                        // Only one return table can exists at a time
                        // Options gotten for each issue *outside* of this loop)
                        var ballotIssueBuilder = new BallotIssue.Builder()
                            .WithSerialNumber(reader.GetString(0))
                            .WithStartDate(reader.GetDateTime(1))
                            .WithEndDate(reader.GetDateTime(2))
                            .WithTitle(reader.GetString(3))
                            .WithDescription(reader.GetString(4));

                        ballotIssueBuilderList.Add(ballotIssueBuilder);
                    }
                    reader.Close();
                }

                //Get options for each issue
                foreach (BallotIssue.Builder builder in ballotIssueBuilderList)
                {
                    List<BallotIssueOption> optionsList = new List<BallotIssueOption>();
                    string? ballotIssueSerialNumber = builder.SerialNumber;

                    using (var cmd = new MySqlCommand("get_options", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("v_serialNumber", ballotIssueSerialNumber);
                        cmd.Parameters["v_serialNumber"].Direction = ParameterDirection.Input;

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var newOption = new BallotIssueOption.Builder()
                                .WithOptionNumber(reader.GetInt32(0))
                                .WithTitle(reader.GetString(1))
                                .Build();

                            optionsList.Add(newOption);
                        }
                        reader.Close();
                    }

                    var issue = builder.WithOptions(optionsList).Build();
                    ballotIssueList.Add(issue);
                }
                return ballotIssueList;
            }
        }

        public List<Voter> GetVoterParticipation(string issueSerial)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_voter_participation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_issueSerial", issueSerial);
                    cmd.Parameters["@v_issueSerial"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'get_voter_participation' with parameters:
    issueSerial: '{issueSerial}'");

                        throw;
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Voter> participants = new();
                        while (reader.Read())
                        {
                            Voter participant = new Voter.Builder()
                                .WithSerialNumber(Convert.ToString(reader.GetString(0)))
                                .WithFirstName(Convert.ToString(reader.GetString(1)))
                                .WithLastName(Convert.ToString(reader.GetString(2)))
                                .WithEmail(Convert.ToString(reader.GetString(3)))
                                .WithUsername("hidden")
                                .WithPassword("H!dd3n")
                                .Build();
                            participants.Add(participant);
                        }
                        return participants;
                    }
                }
            }
        }

        public Dictionary<int, int> GetIssueResults(string issueSerial)
        {

            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_issue_results", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_issueSerial", issueSerial);
                    cmd.Parameters["@v_issueSerial"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'get_voter_participation' with parameters:
    issueSerial: '{issueSerial}'");

                        throw;
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        var results = new Dictionary<int, int>();
                        while (reader.Read())
                        {
                            int optionNumber = Convert.ToInt32(reader.GetValue(0));
                            int count = Convert.ToInt32(reader.GetValue(1));
                            results.Add(optionNumber, count);
                        }
                        return results;
                    }
                }
            }
        }

        public Dictionary<string,Dictionary<int, int>> GetAllResults(List<BallotIssue> issues)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_all_results", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'get_election_results'");
                        throw;
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        var results = MakeMapFromList(ref issues);
                        while (reader.Read())
                        {
                            string issueSerial = reader.GetString(0);
                            int optionNumber = reader.GetInt32(1);
                            int count = reader.GetInt32(2);

                            results[issueSerial][optionNumber] = count;
                        }
                        if (!VerifyResults(results))
                            throw new Exception("Issues from results from db to not match inputed issues");
                        return results;
                    }
                }
            }
        }

        private static Dictionary<string, Dictionary<int, int>> MakeMapFromList(ref List<BallotIssue> issues)
        {
            Dictionary<string, Dictionary<int, int>> map = new ();
            foreach (BallotIssue issue in issues)
            {
                Dictionary<int, int> optionVotes = new();
                foreach (BallotIssueOption option in issue.Options)
                {
                    optionVotes.Add(option.Number, -1);
                }
                map.Add(issue.SerialNumber, optionVotes);
            }
            return map;
        }

        private static bool VerifyResults(Dictionary<string, Dictionary<int, int>> results)
        {
            foreach (var i in results.Keys)
                foreach (var j in results[i].Keys)
                    if (results[i][j] < 0)
                    {
                        Console.WriteLine(@$"Verify map failure: issue {i}, option {j}, count {results[i][j]}");
                        return false;
                    }
            return true;
        }
    }
}