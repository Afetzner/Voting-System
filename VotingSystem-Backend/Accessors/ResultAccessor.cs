using VotingSystem.Model;
using System.Data;
using MySql.Data.MySqlClient;

namespace VotingSystem.Accessor
{
    public class ResultAccessor : IResultAccessor
    { 
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

        public Dictionary<string,Dictionary<int, int>> GetResults(List<BallotIssue> issues)
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

                using (var cmd = new MySqlCommand("get_election_results", conn))
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
                        var results = MakeMapFromList(issues);
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

        /// <summary>
        /// Initalizes a list of ballot options into a dict of dicts: issue-serial -> (option # -> vote count)
        /// </summary>
        private Dictionary<string, Dictionary<int, int>> MakeMapFromList(List<BallotIssue> issues)
        {
            Dictionary<string, Dictionary<int, int>> map = new Dictionary<string,Dictionary<int, int>>();
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

        /// <summary>
        /// Verfifies that each option for each issue has non-negative value
        /// </summary>
        private bool VerifyResults(Dictionary<string, Dictionary<int, int>> results)
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