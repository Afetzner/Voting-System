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
                            Voter participant = new Voter.VoterBuilder()
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

                using (var cmd = new MySqlCommand("get_election_results", conn))
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
    }
}