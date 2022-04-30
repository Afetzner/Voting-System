using VotingSystem.Accessor;
using System;
using VotingSystem.Model;
using System.Data;
using MySql.Data.MySqlClient;

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

                    cmd.Parameters.AddWithValue("v_voterSerial", ballot.Voter.SerialNumber);
                    cmd.Parameters["@v_voterSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_issueSerial", ballot.Issue.SerialNumber);
                    cmd.Parameters["@v_issueSerial"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_choiceNumber", ballot.Choice);
                    cmd.Parameters["@v_choiceNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_collision", MySqlDbType.Byte);
                    cmd.Parameters["@v_collision"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e.ErrorCode);
                        Console.WriteLine(e + "\n\n"
                            + $@"Could not execute SQL procedure 'add_ballot' with parameters:
                            ballot serialNumber: '{ballot.SerialNumber}',
                            voter serialNumber: '{ballot.Voter.SerialNumber}',
                            issue serialNumber: '{ballot.Issue.SerialNumber}',
                            ballot choiceNumber: '{ballot.Choice}'");
                        throw;
                    }
                    return Convert.ToBoolean(cmd.Parameters["v_collision"].Value);
                }
            }
        }

        public List<Ballot> GetBallotsByVoter(string voterSerial)
        {
            List<Ballot> ballots = new List<Ballot>();

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

                    cmd.Parameters.Add("v_ballotSerial", MySqlDbType.VarChar);
                    cmd.Parameters["@v_ballotSerial"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_choiceNumber", MySqlDbType.Int32);
                    cmd.Parameters["@v_choiceNumber"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_choiceTitle", MySqlDbType.VarChar);
                    cmd.Parameters["@v_choiceTitle"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"\nCould not execute SQL procedure get_voters_ballot' with parameters:
                                                    voterSerial: '{voterSerial}'");

                        throw;
                    }
                }
            }
            throw new NotImplementedException();

        }





        public List<BallotIssue> GetIssuesVotedOn(string voterSerial)
            {
                List<BallotIssue> issues = new List<BallotIssue>();

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

               
            }
                throw new NotImplementedException();
            /*
            CREATE PROCEDURE afetzner.get_did_voter_participate(
            IN `v_voterSerial` varchar(9),
            IN `v_issueSerial` varchar(9),
            OUT `v_didVote` bool)
            BEGIN
            */
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
                        cmd.Parameters["@v_occupied"].Direction = ParameterDirection.Input;

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
        }
    }
