using VotingSystem.Accessor;
using System;
using VotingSystem.Model;
using VotingSystem.Utils;
using System.Data;
using MySql.Data.MySqlClient;
using VotingSystem.Controller;

namespace VotingSystem.Model
{
    public class BallotAccessor : IBallotAccessor
    {
        public bool AddBallot(Ballot ballot)
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

                using (var cmd = new MySqlCommand("add_ballot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_ballotSerial", ballot.SerialNumber);
                    cmd.Parameters["@v_ballotSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_voterSerial", ballot.VoterSerial);
                    cmd.Parameters["@v_voterSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_issueSerial", ballot.IssueSerial);
                    cmd.Parameters["@v_issueSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_choiceNumber", ballot.Choice);
                    cmd.Parameters["@v_choiceNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_collision", MySqlDbType.Byte);
                    cmd.Parameters["@v_collision"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e) when (e.ErrorCode == -2147467259)
                    {
                        //Duplicate entry, don't throw, handled by return vlaue
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n\n"
                            + $@"Could not execute SQL procedure 'add_ballot' with parameters:
                            ballot serialNumber: '{ballot.SerialNumber}',
                            voter serialNumber: '{ballot.VoterSerial}',
                            issue serialNumber: '{ballot.IssueSerial}',
                            ballot choiceNumber: '{ballot.Choice}'");
                        throw;
                    }
                    return Convert.ToBoolean(cmd.Parameters["v_collision"].Value);
                }
            }
        }

        public Ballot? GetBallot(string voterSerial, string issueSerial)
        {
            var ballotBuilder = new Ballot.Builder()
                .WithVoter(voterSerial)
                .WithIssue(issueSerial);

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

                using (var cmd = new MySqlCommand("get_voters_ballot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_voterSerial", voterSerial);
                    cmd.Parameters["v_voterSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_issueSerial", issueSerial);
                    cmd.Parameters["v_issueSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_ballotSerial", MySqlDbType.VarChar);
                    cmd.Parameters["@v_ballotSerial"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_choiceNumber", MySqlDbType.Int32);
                    cmd.Parameters["@v_choiceNumber"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_choiceTitle", MySqlDbType.VarChar);
                    cmd.Parameters["@v_choiceTitle"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_isNull", MySqlDbType.Byte);
                    cmd.Parameters["@v_isNull"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"\nCould not execute SQL procedure get_voters_ballot' with parameters:
    voterSerial: '{voterSerial}',
    issueSerial: '{issueSerial}'");

                        throw;
                    }

                    //No vote on that issue
                    if (Convert.ToBoolean(cmd.Parameters["v_isNull"].Value))
                        return null;

                    int choice = Convert.ToInt32(cmd.Parameters["v_choiceNumber"].Value);
                    string? title = Convert.ToString(cmd.Parameters["v_choiceTitle"].Value);

                    return ballotBuilder
                        .WithSerialNumber(Convert.ToString(cmd.Parameters["v_ballotSerial"].Value))
                        .WithChoice(choice)
                        .Build();   
                }
            }
        }

        public void RemoveBallot(string serial)
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

                using (var cmd = new MySqlCommand("delete_ballot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_ballotSerial", serial);
                    cmd.Parameters["@v_ballotSerial"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'delete_ballot' with parameters:
                                                    serialNumber: '{serial}' ");

                        throw;
                    }
                }
            }
        }

        public bool IsSerialInUse(string ballotSerial)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCound not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("check_ballot_serial", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_ballotSerial", ballotSerial);
                    cmd.Parameters["@v_ballotSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_occupied", MySqlDbType.Byte);
                    cmd.Parameters["@v_occupied"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'check_ballot_serial' with parameters:
                                            serialNumber: '{ballotSerial}'");
                        throw;
                    }
                    return Convert.ToBoolean(cmd.Parameters["v_occupied"].Value);
                }
            }
        }

        public string GetSerial()
        {
            string serial;
            do
            {
                serial = SerialGenerator.Generate('B');
            } while (IsSerialInUse(serial));

            return serial;
        }
    }
}
